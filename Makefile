## Makefile Settings
.PHONY: help clean format build
.DEFAULT_GOAL := help
# Detect build machine's OS
ifeq ($(shell uname), Linux)
	MACHINE_OS ?= lnx
	PYTHON_BINDIR = bin
	GDUNIT = ./addons/gdUnit4/runtest.sh -a tests
endif
ifeq ($(shell uname), Darwin)
	MACHINE_OS ?= mac
	PYTHON_BINDIR = bin
	GDUNIT = ./addons/gdUnit4/runtest.sh -a tests
endif
ifndef MACHINE_OS
	MACHINE_OS ?= win
	PYTHON_BINDIR = Scripts
	GDUNIT = .\addons\gdUnit4\runtest.cmd -a tests
endif

## Variables
# Name of Game
GAME ?= SilverAndSteel
# Godot executable path; uses GODOT_BIN environment variable, if present (used by gdUnit)
GODOT_BIN ?= godot
export GODOT_BIN
# dotnet executable path; uses DOTNET environment variable, if present
DOTNET ?= dotnet
# python 3 executable path; uses PYTHON environment variable, if present
PYTHON ?= python
# Git executable path; uses GIT environment variable, if present
GIT ?= git

##@ Build
build:  ## Compile code
	"${DOTNET}" build

export: export.${MACHINE_OS}  ## Export for the current platform

export.win:  ## Export for Windows Desktop
	mkdir -p build/windows
	"${GODOT_BIN}" --headless --export-release "Windows Desktop" "build/windows/${GAME}.exe"

##@ Quality
format: ## Formats the sources
	"${DOTNET}" format "${GAME}.sln"
	.venv/${PYTHON_BINDIR}/gdformat .

lint: ## Runs static analysis
	"${DOTNET}" build "${GAME}.csproj" --configuration Lint
	"${DOTNET}" format "${GAME}.sln" --verify-no-changes
	.venv/${PYTHON_BINDIR}/gdlint .
	.venv/${PYTHON_BINDIR}/format --check .

test: ## Runs automated tests
	${GDUNIT}

##@ Utility
help:  ## Display this help
	@awk 'BEGIN {FS = ":.*##"; printf "\nUsage:\n  make \033[36m\033[0m\n"} /^[$$()% a-zA-Z0-9._-]+:.*?##/ { printf "  \033[36m%-15s\033[0m %s\n", $$1, $$2 } /^##@/ { printf "\n\033[1m%s\033[0m\n", substr($$0, 5) } ' $(MAKEFILE_LIST)

bootstrap: .bootstrap.${MACHINE_OS}  ## Perform once-per-machine setup (install required applications globally)
.bootstrap.win:
	powershell -Command tools/bootstrap/install.ps1
.bootstrap.lnx:
	bash tools/bootstrapping/install.sh
.bootstrap.mac:
	bash tools/bootstrapping/install.sh

setup:  ## Perform once-per-repo-clone setup (download per-project dependencies)
	"${GIT}" lfs install
	"${GIT}" submodule update --init --recursive
	"${PYTHON}" -m venv .venv
	.venv/${PYTHON_BINDIR}/python -m pip install "gdtoolkit==4.2.*"

clean:  ## Delete build artifacts
	"${DOTNET}" clean
	rm -rf build

distclean: clean  ## Restores repo to fresh checkout
	"${GIT}" clean -xdf
