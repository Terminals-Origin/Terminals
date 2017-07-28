FROM ubuntu

MAINTAINER Terminals
LABEL Description="This image used for testing Telnet connections. Allows two admin telnect connections, exposed default telent port 23." \
      Vendor="Terminals.codeplex.com" \
      Version="1.0"

EXPOSE 23

RUN apt-get update && \
    apt-get install -y telnetd && \
    apt-get install -y xinetd  && \
    apt-get install -y net-tools && \
    echo "pts/0" >> /etc/securetty && \
    echo "pts/1" >> /etc/securetty && \
    echo "root:password" | chpasswd
    

CMD /etc/init.d/xinetd restart && bash