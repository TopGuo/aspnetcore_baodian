## һ.ǰ��
.NET Core ������һЩAPI�����ǻ�ȡ����ϵͳ������ʱ����ܵ���Ϣ��
��ЩAPI���Ǻܳ��ã�������ЩС�����ܻ���֪����������һЩ�����õ��Ļ�ȡ����ϵͳ��API����

## ��.�жϲ���ϵͳ
�жϲ���ϵͳ�Ƿ�Ϊ Linux OSX Windows����Ҫʹ�� System.Runtime.InteropServices.IsOSPlatform()���������ʹ�����£�

Console.WriteLine("�ж��Ƿ�ΪWindows Linux OSX");
Console.WriteLine($"Linux:{RuntimeInformation.IsOSPlatform(OSPlatform.Linux)}");
Console.WriteLine($"OSX:{RuntimeInformation.IsOSPlatform(OSPlatform.OSX)}");
Console.WriteLine($"Windows:{RuntimeInformation.IsOSPlatform(OSPlatform.Windows)}");
ִ�н����

1526923666756

## ��.��ȡ����ϵͳ�ܹ�������
Console.WriteLine($"ϵͳ�ܹ���{RuntimeInformation.OSArchitecture}");
Console.WriteLine($"ϵͳ���ƣ�{RuntimeInformation.OSDescription}");
Console.WriteLine($"���̼ܹ���{RuntimeInformation.ProcessArchitecture}");
Console.WriteLine($"�Ƿ�64λ����ϵͳ��{Environment.Is64BitOperatingSystem}");
ִ�н����

1526923737607

��.д�����
��ȡ��Щ��Ϣ������Ҫ���� System.Runtime.InteropServices���ƿռ��¡�
����������� Runtime ���� Environment���������������������ȥ������ҡ�