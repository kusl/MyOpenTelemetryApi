what does it mean by this and how do I fix it? 

Rebuild started at 5:59 AM...
Restored C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Domain\MyOpenTelemetryApi.Domain.csproj (in 136 ms).
Restored C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Application\MyOpenTelemetryApi.Application.csproj (in 139 ms).
Restored C:\code\MyOpenTelemetryApi\tests\MyOpenTelemetryApi.Application.Tests\MyOpenTelemetryApi.Application.Tests.csproj (in 158 ms).
Restored C:\code\MyOpenTelemetryApi\tests\MyOpenTelemetryApi.Infrastructure.Tests\MyOpenTelemetryApi.Infrastructure.Tests.csproj (in 159 ms).
Restored C:\code\MyOpenTelemetryApi\tests\MyOpenTelemetryApi.Api.Tests\MyOpenTelemetryApi.Api.Tests.csproj (in 179 ms).
1>------ Rebuild All started: Project: MyOpenTelemetryApi.Domain, Configuration: Debug Any CPU ------
Restored C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\MyOpenTelemetryApi.Infrastructure.csproj (in 186 ms).
Restored C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\MyOpenTelemetryApi.Api.csproj (in 187 ms).
1>C:\Program Files\dotnet\sdk\10.0.100-preview.6.25358.103\Sdks\Microsoft.NET.Sdk\targets\Microsoft.NET.RuntimeIdentifierInference.targets(335,5): message NETSDK1057: You are using a preview version of .NET. See: https://aka.ms/dotnet-support-policy
1>MyOpenTelemetryApi.Domain -> C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Domain\bin\Debug\net9.0\MyOpenTelemetryApi.Domain.dll
2>------ Rebuild All started: Project: MyOpenTelemetryApi.Infrastructure, Configuration: Debug Any CPU ------
3>------ Rebuild All started: Project: MyOpenTelemetryApi.Application, Configuration: Debug Any CPU ------
3>C:\Program Files\dotnet\sdk\10.0.100-preview.6.25358.103\Sdks\Microsoft.NET.Sdk\targets\Microsoft.NET.RuntimeIdentifierInference.targets(335,5): message NETSDK1057: You are using a preview version of .NET. See: https://aka.ms/dotnet-support-policy
2>MyOpenTelemetryApi.Infrastructure -> C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
4>------ Rebuild All started: Project: MyOpenTelemetryApi.Infrastructure.Tests, Configuration: Debug Any CPU ------
4>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277: Found conflicts between different versions of "Microsoft.EntityFrameworkCore.Relational" that could not be resolved.
4>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277: There was a conflict between "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" and "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60".
4>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:     "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was chosen because it was primary and "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was not.
4>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:     References which depend on "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll].
4>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:         C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
4>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:           Project file item includes which caused reference "C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll".
4>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:             C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
4>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:     References which depend on or have been unified to "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [].
4>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:         C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
4>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:           Project file item includes which caused reference "C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll".
4>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:             C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
4>MyOpenTelemetryApi.Infrastructure.Tests -> C:\code\MyOpenTelemetryApi\tests\MyOpenTelemetryApi.Infrastructure.Tests\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.Tests.dll
4>Done building project "MyOpenTelemetryApi.Infrastructure.Tests.csproj".
3>MyOpenTelemetryApi.Application -> C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Application\bin\Debug\net9.0\MyOpenTelemetryApi.Application.dll
5>------ Rebuild All started: Project: MyOpenTelemetryApi.Application.Tests, Configuration: Debug Any CPU ------
6>------ Rebuild All started: Project: MyOpenTelemetryApi.Api, Configuration: Debug Any CPU ------
5>MyOpenTelemetryApi.Application.Tests -> C:\code\MyOpenTelemetryApi\tests\MyOpenTelemetryApi.Application.Tests\bin\Debug\net9.0\MyOpenTelemetryApi.Application.Tests.dll
6>MyOpenTelemetryApi.Api -> C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll
7>------ Rebuild All started: Project: MyOpenTelemetryApi.Api.Tests, Configuration: Debug Any CPU ------
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277: Found conflicts between different versions of "Microsoft.EntityFrameworkCore.Relational" that could not be resolved.
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277: There was a conflict between "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" and "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60".
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:     "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was chosen because it was primary and "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was not.
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:     References which depend on "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll].
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:         C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:           Project file item includes which caused reference "C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll".
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:             C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:     References which depend on or have been unified to "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [].
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:         C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:           Project file item includes which caused reference "C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll".
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:             C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:         C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:           Project file item includes which caused reference "C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll".
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:             C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
7>C:\Program Files\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\amd64\Microsoft.Common.CurrentVersion.targets(2433,5): warning MSB3277:             C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll
7>MyOpenTelemetryApi.Api.Tests -> C:\code\MyOpenTelemetryApi\tests\MyOpenTelemetryApi.Api.Tests\bin\Debug\net9.0\MyOpenTelemetryApi.Api.Tests.dll
7>Done building project "MyOpenTelemetryApi.Api.Tests.csproj".
========== Rebuild All: 7 succeeded, 0 failed, 0 skipped ==========
========== Rebuild completed at 5:59 AM and took 07.116 seconds ==========

PS C:\code\MyOpenTelemetryApi> Set-Location "C:\Code\MyOpenTelemetryApi\"; Get-Date -Format "yyyy-MM-dd HH:mm:ss"; dotnet restore; Get-Date -Format "yyyy-MM-dd HH:mm:ss"; dotnet clean; Get-Date -Format "yyyy-MM-dd HH:mm:ss"; dotnet build; Get-Date -Format "yyyy-MM-dd HH:mm:ss"; dotnet test; Get-Date -Format "yyyy-MM-dd HH:mm:ss"; git status; Get-Date -Format "yyyy-MM-dd HH:mm:ss"; git remote show origin; Get-Date -Format "yyyy-MM-dd HH:mm:ss";
2025-08-05 06:00:50
Restore complete (2.6s)

Build succeeded in 2.9s
2025-08-05 06:00:54
You are using a preview version of .NET. See: https://aka.ms/dotnet-support-policy

Build succeeded in 2.0s
2025-08-05 06:00:56
Restore complete (2.0s)
You are using a preview version of .NET. See: https://aka.ms/dotnet-support-policy
  MyOpenTelemetryApi.Domain succeeded (0.6s) → src\MyOpenTelemetryApi.Domain\bin\Debug\net9.0\MyOpenTelemetryApi.Domain.dll
  MyOpenTelemetryApi.Application succeeded (1.1s) → src\MyOpenTelemetryApi.Application\bin\Debug\net9.0\MyOpenTelemetryApi.Application.dll
  MyOpenTelemetryApi.Infrastructure succeeded (1.3s) → src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
  MyOpenTelemetryApi.Application.Tests succeeded (0.8s) → tests\MyOpenTelemetryApi.Application.Tests\bin\Debug\net9.0\MyOpenTelemetryApi.Application.Tests.dll
  MyOpenTelemetryApi.Infrastructure.Tests succeeded with 1 warning(s) (1.9s) → tests\MyOpenTelemetryApi.Infrastructure.Tests\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.Tests.dll
    C:\Program Files\dotnet\sdk\10.0.100-preview.6.25358.103\Microsoft.Common.CurrentVersion.targets(2438,5): warning MSB3277:
      Found conflicts between different versions of "Microsoft.EntityFrameworkCore.Relational" that could not be resolved.
      There was a conflict between "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" and "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0,
      Culture=neutral, PublicKeyToken=adb9793829ddae60".
          "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was chosen because it was primary and "Microsoft.EntityFrameworkCore.Relational, Version=
      9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was not.
          References which depend on "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore
      .relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll].
              C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
                Project file item includes which caused reference "C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll".
                  C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
          References which depend on or have been unified to "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [].
              C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
                Project file item includes which caused reference "C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll".
                  C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
  MyOpenTelemetryApi.Api succeeded (4.1s) → src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll
  MyOpenTelemetryApi.Api.Tests succeeded with 1 warning(s) (1.7s) → tests\MyOpenTelemetryApi.Api.Tests\bin\Debug\net9.0\MyOpenTelemetryApi.Api.Tests.dll
    C:\Program Files\dotnet\sdk\10.0.100-preview.6.25358.103\Microsoft.Common.CurrentVersion.targets(2438,5): warning MSB3277:
      Found conflicts between different versions of "Microsoft.EntityFrameworkCore.Relational" that could not be resolved.
      There was a conflict between "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" and "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0,
      Culture=neutral, PublicKeyToken=adb9793829ddae60".
          "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was chosen because it was primary and "Microsoft.EntityFrameworkCore.Relational, Version=
      9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was not.
          References which depend on "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore
      .relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll].
              C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
                Project file item includes which caused reference "C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll".
                  C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
          References which depend on or have been unified to "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [].
              C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll
                Project file item includes which caused reference "C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll".
                  C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll
              C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
                Project file item includes which caused reference "C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll".
                  C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
                  C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll

Build succeeded with 2 warning(s) in 9.7s
2025-08-05 06:01:07
Restore complete (2.1s)
You are using a preview version of .NET. See: https://aka.ms/dotnet-support-policy
  MyOpenTelemetryApi.Domain succeeded (0.2s) → src\MyOpenTelemetryApi.Domain\bin\Debug\net9.0\MyOpenTelemetryApi.Domain.dll
  MyOpenTelemetryApi.Infrastructure succeeded (0.3s) → src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
  MyOpenTelemetryApi.Application succeeded (0.3s) → src\MyOpenTelemetryApi.Application\bin\Debug\net9.0\MyOpenTelemetryApi.Application.dll
  MyOpenTelemetryApi.Application.Tests succeeded (0.4s) → tests\MyOpenTelemetryApi.Application.Tests\bin\Debug\net9.0\MyOpenTelemetryApi.Application.Tests.dll
  MyOpenTelemetryApi.Infrastructure.Tests succeeded with 1 warning(s) (0.4s) → tests\MyOpenTelemetryApi.Infrastructure.Tests\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.Tests.dll
    C:\Program Files\dotnet\sdk\10.0.100-preview.6.25358.103\Microsoft.Common.CurrentVersion.targets(2438,5): warning MSB3277:
      Found conflicts between different versions of "Microsoft.EntityFrameworkCore.Relational" that could not be resolved.
      There was a conflict between "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" and "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0,
      Culture=neutral, PublicKeyToken=adb9793829ddae60".
          "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was chosen because it was primary and "Microsoft.EntityFrameworkCore.Relational, Version=
      9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was not.
          References which depend on "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore
      .relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll].
              C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
                Project file item includes which caused reference "C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll".
                  C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
          References which depend on or have been unified to "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [].
              C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
                Project file item includes which caused reference "C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll".
                  C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
  MyOpenTelemetryApi.Api succeeded (1.4s) → src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll
  MyOpenTelemetryApi.Api.Tests succeeded with 1 warning(s) (0.3s) → tests\MyOpenTelemetryApi.Api.Tests\bin\Debug\net9.0\MyOpenTelemetryApi.Api.Tests.dll
    C:\Program Files\dotnet\sdk\10.0.100-preview.6.25358.103\Microsoft.Common.CurrentVersion.targets(2438,5): warning MSB3277:
      Found conflicts between different versions of "Microsoft.EntityFrameworkCore.Relational" that could not be resolved.
      There was a conflict between "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" and "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0,
      Culture=neutral, PublicKeyToken=adb9793829ddae60".
          "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was chosen because it was primary and "Microsoft.EntityFrameworkCore.Relational, Version=
      9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" was not.
          References which depend on "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore
      .relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll].
              C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
                Project file item includes which caused reference "C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll".
                  C:\Users\kushal\.nuget\packages\microsoft.entityframeworkcore.relational\9.0.1\lib\net8.0\Microsoft.EntityFrameworkCore.Relational.dll
          References which depend on or have been unified to "Microsoft.EntityFrameworkCore.Relational, Version=9.0.8.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" [].
              C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll
                Project file item includes which caused reference "C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll".
                  C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll
              C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
                Project file item includes which caused reference "C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll".
                  C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Infrastructure\bin\Debug\net9.0\MyOpenTelemetryApi.Infrastructure.dll
                  C:\code\MyOpenTelemetryApi\src\MyOpenTelemetryApi.Api\bin\Debug\net9.0\MyOpenTelemetryApi.Api.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.3+b1b99bdeb3 (64-bit .NET 9.0.8)
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.3+b1b99bdeb3 (64-bit .NET 9.0.8)
[xUnit.net 00:00:00.93]   Discovering: MyOpenTelemetryApi.Application.Tests
[xUnit.net 00:00:00.97]   Discovering: MyOpenTelemetryApi.Infrastructure.Tests
[xUnit.net 00:00:01.10]   Discovered:  MyOpenTelemetryApi.Infrastructure.Tests
[xUnit.net 00:00:01.13]   Discovered:  MyOpenTelemetryApi.Application.Tests
[xUnit.net 00:00:01.17]   Starting:    MyOpenTelemetryApi.Infrastructure.Tests
[xUnit.net 00:00:01.22]   Starting:    MyOpenTelemetryApi.Application.Tests
[xUnit.net 00:00:01.74]   Finished:    MyOpenTelemetryApi.Infrastructure.Tests
[xUnit.net 00:00:01.80]   Finished:    MyOpenTelemetryApi.Application.Tests
  MyOpenTelemetryApi.Infrastructure.Tests test succeeded (4.5s)
  MyOpenTelemetryApi.Application.Tests test succeeded (4.6s)
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.3+b1b99bdeb3 (64-bit .NET 9.0.8)
[xUnit.net 00:00:00.79]   Discovering: MyOpenTelemetryApi.Api.Tests
[xUnit.net 00:00:00.89]   Discovered:  MyOpenTelemetryApi.Api.Tests
[xUnit.net 00:00:00.95]   Starting:    MyOpenTelemetryApi.Api.Tests
[xUnit.net 00:00:01.49]   Finished:    MyOpenTelemetryApi.Api.Tests
  MyOpenTelemetryApi.Api.Tests test succeeded (4.3s)

Test summary: total: 44, failed: 0, succeeded: 44, skipped: 0, duration: 6.3s
Build succeeded with 2 warning(s) in 9.7s
2025-08-05 06:01:17
On branch main
Your branch is ahead of 'origin/main' by 3 commits.
  (use "git push" to publish your local commits)

Changes not staged for commit:
  (use "git add <file>..." to update what will be committed)
  (use "git restore <file>..." to discard changes in working directory)
        modified:   src/MyOpenTelemetryApi.Api/appsettings.json

no changes added to commit (use "git add" and/or "git commit -a")
2025-08-05 06:01:17
* remote origin
  Fetch URL: git@github.com:kusl/MyOpenTelemetryApi.git
  Push  URL: git@github.com:kusl/MyOpenTelemetryApi.git
  HEAD branch: main
  Remote branches:
    dependabot/github_actions/codecov/codecov-action-5                         new (next fetch will store in remotes/origin)
    dependabot/github_actions/danielpalme/ReportGenerator-GitHub-Action-5.4.11 new (next fetch will store in remotes/origin)
    dependabot/nuget/src/MyOpenTelemetryApi.Api/microsoft-packages-25694566e4  new (next fetch will store in remotes/origin)
    main                                                                       tracked
  Local branch configured for 'git pull':
    main merges with remote main
  Local ref configured for 'git push':
    main pushes to main (fast-forwardable)
2025-08-05 06:01:18
PS C:\code\MyOpenTelemetryApi>






The warnings about `Microsoft.EntityFrameworkCore.Relational` version conflicts mean that different packages in your project are referencing different versions of this dependency:

- Some packages want version **9.0.1**
- Your projects are using version **9.0.8**

This happens because:
1. The OpenTelemetry EF Core instrumentation package (`OpenTelemetry.Instrumentation.EntityFrameworkCore`) is still in beta and references an older version
2. Your main projects have newer versions

To fix this, you need to align all Entity Framework Core packages to the same version. Here's how:

**Option 1: Force all packages to use the same version (Recommended)**

Add explicit package references to your `.csproj` files to override the transitive dependencies:

```powershell
# Update Infrastructure project
cd src/MyOpenTelemetryApi.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Relational --version 9.0.0
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.0

# Update API project  
cd ../MyOpenTelemetryApi.Api
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0

cd ../..
```

**Option 2: Use a Directory.Build.props file to centralize versions**

Create a `Directory.Build.props` file in your solution root:

```xml
<Project>
  <PropertyGroup>
    <EFCoreVersion>9.0.0</EFCoreVersion>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Update="Microsoft.EntityFrameworkCore" Version="$(EFCoreVersion)" />
    <PackageReference Update="Microsoft.EntityFrameworkCore.Design" Version="$(EFCoreVersion)" />
    <PackageReference Update="Microsoft.EntityFrameworkCore.Relational" Version="$(EFCoreVersion)" />
    <PackageReference Update="Npgsql.EntityFrameworkCore.PostgreSQL" Version="$(EFCoreVersion)" />
  </ItemGroup>
</Project>
```

**Option 3: Update the OpenTelemetry instrumentation package**

Check if there's a newer version that supports EF Core 9.0.8:

```powershell
cd src/MyOpenTelemetryApi.Api
dotnet list package --outdated --include-prerelease
```

The warnings don't prevent your app from running, but they indicate potential compatibility issues. The build chooses version 9.0.1 to be safe, which might mean you're not getting bug fixes from newer versions.

Also, I notice you have uncommitted changes to `appsettings.json` - that's likely your connection string placeholder change that needs to be committed.