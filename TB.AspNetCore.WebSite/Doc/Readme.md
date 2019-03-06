##  数据库
dotnet ef dbcontext scaffold "Server=35.240.208.158;Database=BaoDian;User=root;Password=YAya123..." "Pomelo.EntityFrameworkCore.MySql"
Scaffold-DbContext "Server=35.240.208.158;Database=BaoDian;User=root;Password=YAya123..." "Pomelo.EntityFrameworkCore.MySql" -OutputDir Entitys -Force


## Linux（CentOS7）下使用 Zkweb.system.drawing--图形验证码
我们按照步骤，执行以下命令：　　

yum install autoconf automake libtool

yum install freetype-devel fontconfig libXft-devel

yum install libjpeg-turbo-devel libpng-devel giflib-devel libtiff-devel libexif-devel

yum install glib2-devel cairo-devel

git clone https://github.com/mono/libgdiplus (如果没有安装git命令 执行yum install git 安装)

wget http://dl.fedoraproject.org/pub/epel/7/x86_64/Packages/l/libgdiplus-2.10-10.el7.x86_64.rpm（如果没有安装wget命令 执行yum install wget）

wget http://dl.fedoraproject.org/pub/epel/7/x86_64/Packages/l/libgdiplus-devel-2.10-9.el7.x86_64.rpm

rpm -Uvh libgdiplus-2.10-9.el7.x86_64.rpm

rpm -Uvh libgdiplus-devel-2.10-9.el7.x86_64.rpm

cd libgdiplus

./autogen.sh

make

make install

cd /usr/lib64/

ln -s /usr/local/lib/libgdiplus.so gdiplus.dll

### 将验证码需要用到的字体文件，你的Linux服务器上可能没有，如果么有则从win上考到Linux上
win上字体路径在C:\Windos\Font
Linux上字体文件，本人的在 /usr/share/fonts/chinese/TrueType

上传到服务器的 /usr/share/fonts/chinese/TrueType 目录下（chinese/TrueType 两个目录是自己创建的）

进入这个目录：

cd /usr/share/fonts/chinese/TrueType

mkfontscale（如果提示 mkfontscale: command not found，需自行安装 # yum install mkfontscale ）

mkfontdir

fc-cache -fv（如果提示 fc-cache: command not found，则需要安装# yum install fontconfig ）


## dbfrist 数据库迁移命令

Scaffold-DbContext "Server=35.240.208.158;Database=RestApi;User=root;Password=YAya123...;" "Pomelo.EntityFrameworkCore.MySql" -Force -OutputDir Entitys

## supervisor

[program:tbaspnetcorexinchenbeta];自定义进程名称
command=dotnet TB.AspNetCore.WebSite.dll;程序启动命令
directory=/home/xingchenbeta;命令执行的目录
autostart=true;在Supervisord启动时，程序是否启动
autorestart=true;程序退出后自动重启
startretries=5;启动失败自动重试次数，默认是3
startsecs=1;自动重启间隔
user=root;设置启动进程的用户，默认是root
priority=999;进程启动优先级，默认999，值小的优先启动
stderr_logfile=/home/netpro/xingchenbeta.err.log;标准错误日志
stdout_logfile=/var/log/xingchen.out.log;标准输出日志
environment=ASPNETCORE_ENVIRONMENT=Production;进程环境变量
stopsignal=INT;请求停止时用来杀死程序的信号

## 配置守护进程

Supervisor 配置守护进程
Supervisor 是用 Python 开发的 Linux/Unix 系统下的一个进程管理工具。它可以使进程脱离终端，变为后台守护进程（daemon）。实时监控进程状态，
异常退出时能自动重启。

Supervisor 不支持任何版本的 Window 系统；仅支持在 Python2.4 或更高版本，但不能在任何版本的 Python 3 下工作。

其主要组成部分：

supervisord：Supervisor 的守护进程服务，用于接收进程管理命令；

supervisorctl：Supervisor 命令行工具，用于和守护进程通信，发送管理进程的指令；

Web Server：Web 端进程管理工具，提供与 supervisorctl 类似功能，管理进程；

XML-RPC Interface：提供 XML-RPC 接口，请参阅 XML-RPC API文档。

安装 Supervisor
联网状态下，官方推荐首选安装方法是使用easy_install，它是setuptools（Python 包管理工具）的一个功能。所以先执行如下命令安装 setuptools：

Copy
yum install python-setuptools
请更换root用户，执行如下命令安装 Supervisor：

Copy
easy_install supervisor
配置 Supervisor
运行supervisord服务的时候，需要指定 Supervisor 配置文件，如果没有显示指定，默认会从以下目录中加载：

Copy
$CWD/supervisord.conf  #$CWD表示运行 supervisord 程序的目录
$CWD/etc/supervisord.conf
/etc/supervisord.conf
/etc/supervisor/supervisord.conf (since Supervisor 3.3.0)
../etc/supervisord.conf (Relative to the executable)
../supervisord.conf (Relative to the executable)
所以，先通过如下命令创建目录，以便让 Supervisor 成功加载默认配置：

Copy
mkdir /etc/supervisor
加载目录有了，然后通过echo_supervisord_conf程序（用来生成初始配置文件）来初始化一个配置文件：

Copy
echo_supervisord_conf > /etc/supervisor/supervisord.conf
打开supervisord.conf文件，可以看到echo_supervisord_conf已经帮我们初始化好了一个样例配置，我们需要简单修改一下。

尾部找到如下文本片段：

Copy
;[include]
;files = relative/directory/*.ini
改为：

Copy
[include]
files = conf.d/*.conf
即，把注释去除、设置/etc/supervisor/conf.d为 Supervisor 进程配置文件加载目录。

这样，Supervisor 会自动加载该目录下.conf后缀的文件作为共同服务配置。Supervisor 管理的每个进程单独写一个配置文件放在该目录下，supervisord.conf配置文件中保留公共配置。

创建进程配置加载目录：

Copy
mkdir /etc/supervisor/conf.d
接下来就需要为我们已经部署的 ASP .NET Core 程序的宿主进程创建一个进程配置文件netcore.conf，保存并上传到/etc/supervisor/conf.d目录。

配置文件netcore.conf内容如下：

Copy
[program:Scorpio.WebApi]                        ;自定义进程名称
command=dotnet Scorpio.WebApi.dll               ;程序启动命令
directory=/home/wwwroot/scorpio                 ;命令执行的目录
autostart=true                                  ;在Supervisord启动时，程序是否启动
autorestart=true                                ;程序退出后自动重启
startretries=5                                  ;启动失败自动重试次数，默认是3
startsecs=1                                     ;自动重启间隔
user=root                                       ;设置启动进程的用户，默认是root
# priority=999                                    ;进程启动优先级，默认999，值小的优先启动
stderr_logfile=/var/log/Scorpio.WebApi.err.log  ;标准错误日志
stdout_logfile=/var/log/Scorpio.WebApi.out.log  ;标准输出日志
environment=ASPNETCORE_ENVIRONMENT=Production   ;进程环境变量
stopsignal=INT                                  ;请求停止时用来杀死程序的信号
启动 Supervisor 服务，命令如下：

Copy
supervisord -c /etc/supervisor/supervisord.conf
这时，在会发现我们部署的网站程序不在 shell 中通过dotnet xxx.dll启动，同样可以访问。

设置 Supervisor 开机启动
首先为 Supervisor 新建一个启动服务脚本supervisor.service，然后保存并上传至服务器/usr/lib/systemd/system/目录。

脚本内容如下：

Copy
# supervisord service for systemd (CentOS 7.0+)
# by ET-CS (https://github.com/ET-CS)
[Unit]
Description=Supervisor daemon

[Service]
Type=forking
ExecStart=/usr/bin/supervisord -c /etc/supervisor/supervisord.conf
ExecStop=/usr/bin/supervisorctl $OPTIONS shutdown
ExecReload=/usr/bin/supervisorctl $OPTIONS reload
KillMode=process
Restart=on-failure
RestartSec=42s

[Install]
WantedBy=multi-user.target
设置开启启动：

Copy
systemctl enable supervisor
验证是否成功：

Copy
systemctl is-enabled supervisor
如果输出enabled则表示设置成功，也可重启服务器验证。

其它 Linux 发行版开机启动脚本 User-contributed OS init scripts for Supervisor

Supervisorctl 管理进程
Supervisor 服务启动后，受其管理的进程会在后台运行。可以通过supervisorctl客户端管理进程。

输入如下命令进入supervisorctl交互终端，按Ctrl+C键退出：

Copy
supervisorctl
输入help查询帮助：

Copy
supervisor> help

default commands (type help <topic>):
=====================================
add    exit      open  reload  restart   start   tail
avail  fg        pid   remove  shutdown  status  update
clear  maintail  quit  reread  signal    stop    version
输入help ****查询详细命令，比如输入help stop：

Copy
supervisor> help stop

stop <name>             Stop a process
stop <gname>:*          Stop all processes in a group
stop <name> <name>      Stop multiple processes or groups
stop all                Stop all processes


## 逆向生产model
