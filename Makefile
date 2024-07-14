## Makefile Settings
.PHONY: bootstrap build clean distclean export export.win format lint test help setup
.DEFAULT_GOAL := help

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
# .venv/Scripts on Windows, .venv/bin in Unix
VENV_PYTHON ?= .venv/bin/python
# Detect build machine's OS
ifeq ($(shell uname), Linux)
	MACHINE_OS ?= lnx
endif
ifeq ($(shell uname), Darwin)
	MACHINE_OS ?= mac
endif
ifndef MACHINE_OS
	MACHINE_OS ?= win
	VENV_PYTHON = .venv/Scripts/python
endif

##@ Build
build:  ## Compile code
	"${DOTNET}" build

export: export.${MACHINE_OS}  ## Export for the current platform

export.win:  ## Export for Windows Desktop
	mkdir -p build/windows
	"${GODOT_BIN}" --headless --export-release "Windows Desktop"

##@ Quality
format: .venv ## Formats the sources
	"${DOTNET}" format "${GAME}.sln"
	"${VENV_PYTHON}" -m gdtoolkit.formatter .

lint: .venv ## Runs static analysis
	"${DOTNET}" build "${GAME}.csproj" --configuration Lint
	"${DOTNET}" format "${GAME}.sln" --verify-no-changes
	"${VENV_PYTHON}" -m gdtoolkit.linter .
	"${VENV_PYTHON}" -m gdtoolkit.formatter --check .

test: ## Runs automated tests
	dotnet test

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

.venv:
	"${PYTHON}" -m venv .venv
	"${VENV_PYTHON}" -m pip install "gdtoolkit==4.2.*"

setup: .venv  ## Perform once-per-repo-clone setup (download per-project dependencies)
	"${GIT}" lfs install
	"${GIT}" submodule update --init --recursive

clean:  ## Delete build artifacts
	"${DOTNET}" clean
	rm -rf build

distclean: clean  ## Restores repo to fresh checkout
	"${GIT}" clean -xdf
