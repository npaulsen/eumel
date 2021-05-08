# [Choice] .NET version: 5.0, 3.1, 2.1
ARG VARIANT=5.0
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:${VARIANT}

RUN apt-get update \
    && apt-get install zsh dos2unix -y

USER vscode
RUN rm -rf ~/.oh-my-zsh \
    && sh -c "$(curl -fsSL https://raw.github.com/ohmyzsh/ohmyzsh/master/tools/install.sh)"

COPY .zshrc /home/vscode/.zshrc
RUN sudo dos2unix /home/vscode/.zshrc

# RUN dotnet tool install --global dotnet-reportgenerator-globaltool

ENTRYPOINT ["/bin/sh","-c"]
CMD ["while sleep 1000; do :; done"]