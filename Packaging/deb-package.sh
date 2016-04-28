# Move to the base folder where the script is located.
cd $(dirname $0)

LIBWARCRAFT_ROOT=".."
OUTPUT_ROOT="$LIBWARCRAFT_ROOT/release"

RED='\033[0;31m'
GREEN='\033[0;32m'
ORANGE='\033[0;33m'
LOG_PREFIX="${GREEN}[libwarcraft]:"
LOG_PREFIX_ORANGE="${ORANGE}[libwarcraft]:"
LOG_PREFIX_RED="${RED}[libwarcraft]:"
LOG_SUFFIX='\033[0m'

echo -e "$LOG_PREFIX Building Release configuration of libwarcraft... $LOG_SUFFIX"
xbuild /p:Configuration="Release" "$LIBWARCRAFT_ROOT/libwarcraft/libwarcraft.csproj"

LIBWARCRAFT_ASSEMBLY_VERSION=$(monodis --assembly "$LIBWARCRAFT_ROOT/libwarcraft/bin/Release/libwarcraft.dll" | grep Version | egrep -o '[0-9]*\.[0-9]*\.[0-9]*\.[0-9]*d*')
LIBWARCRAFT_MAJOR_VERSION=$(echo "$LIBWARCRAFT_ASSEMBLY_VERSION" | awk -F \. {'print $1'})
LIBWARCRAFT_MINOR_VERSION=$(echo "$LIBWARCRAFT_ASSEMBLY_VERSION" | awk -F \. {'print $2'})

LIBWARCRAFT_DEBUILD_ROOT="libwarcraft-$LIBWARCRAFT_MAJOR_VERSION.$LIBWARCRAFT_MINOR_VERSION"

if [ -d "$LIBWARCRAFT_DEBUILD_ROOT" ]; then
	rm -rf "$LIBWARCRAFT_DEBUILD_ROOT"
fi

# Copy the sources to the build directory
mkdir "$LIBWARCRAFT_DEBUILD_ROOT"
cp -r "$LIBWARCRAFT_ROOT/libwarcraft/" "libwarcraft-$LIBWARCRAFT_MAJOR_VERSION.$LIBWARCRAFT_MINOR_VERSION"

# Create an *.orig.tar.xz archive if one doesn't exist already
