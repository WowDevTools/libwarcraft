#!/bin/bash

# Move to the base folder where the script is located.
cd $(dirname $0)

LIBWARCRAFT_ROOT=$(readlink -f "..")
OUTPUT_ROOT="$LIBWARCRAFT_ROOT/release"

RED='\033[0;31m'
GREEN='\033[0;32m'
ORANGE='\033[0;33m'
LOG_PREFIX="${GREEN}[libwarcraft]:"
LOG_PREFIX_ORANGE="${ORANGE}[libwarcraft]:"
LOG_PREFIX_RED="${RED}[libwarcraft]:"
LOG_SUFFIX='\033[0m'

echo -e "$LOG_PREFIX Building Release configuration of libwarcraft... $LOG_SUFFIX"
BUILDSUCCESS=$(xbuild /p:Configuration="Release" "$LIBWARCRAFT_ROOT/libwarcraft.sln"  | grep "Build succeeded.")

if [[ ! -z $BUILDSUCCESS ]]; then
	echo "Build succeeded. Copying files and building package."
	# The library builds, so we can proceed
	LIBWARCRAFT_ASSEMBLY_VERSION=$(monodis --assembly "$LIBWARCRAFT_ROOT/libwarcraft/bin/Release/libwarcraft.dll" | grep Version | egrep -o '[0-9]*\.[0-9]*\.[0-9]*\.[0-9]*d*')
	LIBWARCRAFT_MAJOR_VERSION=$(echo "$LIBWARCRAFT_ASSEMBLY_VERSION" | awk -F \. {'print $1'})
	LIBWARCRAFT_MINOR_VERSION=$(echo "$LIBWARCRAFT_ASSEMBLY_VERSION" | awk -F \. {'print $2'})

	LIBWARCRAFT_VERSIONED_NAME="libwarcraft$LIBWARCRAFT_MAJOR_VERSION.$LIBWARCRAFT_MINOR_VERSION-$LIBWARCRAFT_ASSEMBLY_VERSION"
	LIBWARCRAFT_TARBALL_NAME="libwarcraft$LIBWARCRAFT_MAJOR_VERSION.$LIBWARCRAFT_MINOR_VERSION-cli_$LIBWARCRAFT_ASSEMBLY_VERSION"
	LIBWARCRAFT_DEBUILD_ROOT="$OUTPUT_ROOT/$LIBWARCRAFT_VERSIONED_NAME"
	
	# Update Debian changelog
	cd $LIBWARCRAFT_ROOT
	dch -v $LIBWARCRAFT_ASSEMBLY_VERSION-1
	cd - > /dev/null

	if [ ! -d "$LIBWARCRAFT_DEBUILD_ROOT" ]; then
		# Clean the sources
		rm -rf "$LIBWARCRAFT_ROOT/libwarcraft/bin"
		rm -rf "$LIBWARCRAFT_ROOT/libwarcraft/obj"
	
		# Copy the sources to the build directory
		mkdir -p "$LIBWARCRAFT_DEBUILD_ROOT"
		cp -r "$LIBWARCRAFT_ROOT/debian/" $LIBWARCRAFT_DEBUILD_ROOT
		cp -r "$LIBWARCRAFT_ROOT/libwarcraft/" $LIBWARCRAFT_DEBUILD_ROOT
		cp -r "$LIBWARCRAFT_ROOT/mono/" $LIBWARCRAFT_DEBUILD_ROOT
		cp "$LIBWARCRAFT_ROOT/"* "$LIBWARCRAFT_DEBUILD_ROOT"

		# Create an *.orig.tar.xz archive if one doesn't exist already
		ORIG_TAR="$OUTPUT_ROOT/$LIBWARCRAFT_TARBALL_NAME.orig.tar.xz"
		if [ ! -f "$ORIG_TAR" ]; then
			cd "$LIBWARCRAFT_DEBUILD_ROOT/"
			tar -cJf "$ORIG_TAR" "."
			cd - > /dev/null
		fi
		
		# Build the debian package
		read -p "Ready to build the debian package. Continue? [y/N] " -n 1 -r
		echo
		if [[ $REPLY =~ ^[Yy]$ ]]
		then
			cd "$LIBWARCRAFT_DEBUILD_ROOT"
			debuild -S -k28C56D2F
		fi							
	fi
else
	echo "The build failed. Aborting."
fi
