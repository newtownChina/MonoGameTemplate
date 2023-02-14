1.在vs里面把该文件夹添加为nuget源。
2.在nuget里面选择该源安装需要的包（这一步会把包复制到nuget默认的config配置的.nugut package目录C:\Users\Administrator\.nuget\package）
3.这个时候查看项目依赖项可以看到已经有依赖存在，而且.csproj文件里面会有对应配置。