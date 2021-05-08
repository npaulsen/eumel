export ZSH=$HOME/.oh-my-zsh

ZSH_THEME="jonathan"
HIST_STAMPS="yyyy-mm-dd"

plugins=(git dotnet)

source $ZSH/oh-my-zsh.sh

DISABLE_AUTO_UPDATE=true
DISABLE_UPDATE_PROMPT=true

DOTNET_CLI_TELEMETRY_OPTOUT=1

export PATH="${PATH}:/home/vscode/.dotnet/tools"
