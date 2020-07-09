Consoleable
===========

A slightly-opinionated `dotnet new` template for a component which may be used
from the commandline or as an class library

Usage:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
dotnet new consoleable [--name MyName] [--xunit] [--nunit] [--sln] [--serilog] [--testbase]
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
# --xunit : also generate a skeleton xunit test project for the new project
# --nunit : also generate a skeleton nunit test project for the new project
# --sln : also generate a solution file referencing the new project(s).
# --serilog : use Serilog for logging
# --testbase : use TestBase fluent assertions for your tests
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

To easily add new projects to an existing solution, run the command from the 
solution directory then use `dotnet sln add ...`.

Confirm your new project builds with `dotnet run` in the project directory.
Run tests with a `dotnet test` in the test or solution directory.

Long Example

```
dotnet new consoleable --name Freddie --xunit --testbase --sln --serilog && cd Freddie && dotnet test && cd Freddie && dotnet run
```

### Opinions?! What Opinions?

These opinions:

-   Anything bigger than ½ a day's work wants testability, instrumentation and 
    configurability, but we really don't want to write all that boilerplate again
    every. single. time.

-   When run from a commandline, you want an `ILoggerFactory`, an `IConfigurationRoot`, 
    and a `Settings` object.

-   Those three things should work straight out of the box when you use the
    template, and then be instantly editable to your own favourite logging,
    configuration and/or settings solution. “Work straight out of the box” means
    that you can `dotnet run` and `dotnet test` immediatly after creating
    the new project.

-   The command-line should do some elementary argument parsing and if
    appropriate show help text and immediately exit.

-   These *command-line Concerns* should stay out of your way and be 
    ever-so-neatly separated from the Concerns of your component's functionality.

### Why is it better than `dotnet new console`?

Because

1.  It cleanly separates the concerns of Your Component from the concerns of
    being a Self-hosted Command-line Tool

2.  It addresses the concerns of being a command-line tool for you, out of the
    box, saving you the grunt work.

3.  You can easily replace the opinions with your own.


### Why is it better than `dotnet new classlib`?

Who doesn't secretly want their components to be independently usable
and testable from the commandline?  


#### Why is it worse than `dotnet new classlib`?

Because it will take you 20 seconds to edit the .csproj files, remove the
`<TargetFramework>netcoreapp3.1</TargetFramework>` and 
uncomment the `<TargetFramework>netstandard2.0</TargetFramework>`

commandline runnability involves targetting `netcoreapp3.1` instead of
`netstandard2`. Otoh, that's a 20 second edit of the `.csproj` file to fix.

### Comments

I borrowed the opinion that a Startup class was a good place to configure a Logger, ConfigurationRoot and Settings from AspNetCore.

Adding my favourite boilerplate (xunit,nunit,serilog,testbase) as commandline options
turned out to be marvellously simple. 

Adding your own favourite boilerplate is only a fork & a pull request away. Unless you have you really obscure minority opinions. In that case, it's only a fork and no pull request away.


### How to install locally

```
git clone https://github.com/chrisfcarroll/Consoleable
dotnet new -i ./Consoleable/
# … do some editing then re-install just by installing the directory again:
dotnet new -i ./Consoleable/ 
```

Handily, `dotnet new -u` will not only list installed templates it will also list the exact command line to uninstall.

## Any Examples?

https://nuget.org/packages/MailMerge was the somewhat messier forerunner to this template.

What Opinions did you carefully avoid?
--------------------------------------

You can add your own choice of logging provider and test framework.


What Opinions did you not carefully avoid?
--------------------------------------

Dotnet rocks, serilog is great, other logging frameworks are also available, 
there's nothing to choose between NUnit & xUnit except taste and 15 years of
assertion helpers, tests should express the specification, and are best written
as WhenXGivenYThenZ, NFRs can be tested, TestBase fluent assertions are also great.
