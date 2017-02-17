FROM ubuntu

MAINTAINER Terminals
LABEL Description="This image used for testing Ssh connections authenticated using certificates. Allows admin connections, exposed default ssh port 22." \
      Vendor="Terminals.codeplex.com" \
      Version="1.0"

EXPOSE 22

RUN apt-get update && \
    apt-get install -y openssh-server

COPY sshd_config /etc/ssh/sshd_config
COPY issue.net /etc/issue.net
COPY id_rsa.pub /root/.ssh/authorized_keys

CMD service ssh start && bash