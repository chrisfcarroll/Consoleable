Consoleable
===========

A slightly-opinionated `dotnet new` template for a component classlib which also runs
from the commandline. The opinions are, opt-in logging, configuration and testing
are good things and should all work out of the box.

## How to install & uninstall templates from NuGet
```
dotnet new --install Consoleable #finds the Consoleable templates on NuGet
dotnet new consoleable --help
#example: dotnet new consoleable --xunit ; dotnet test ; dotnet run'
dotnet new -u Consoleable
```

#### Or, install & uninstall locally and edit to taste

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
git clone https://github.com/chrisfcarroll/Consoleable
dotnet new -i ./Consoleable/Templates
dotnet new consoleable --help
# … do some editing … then re-install just by installing the directory again:
dotnet new -u ./Consoleable/Templates
dotnet new -i ./Consoleable/Templates
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

`dotnet new -u` with no path will neatly tell you the exact command to uninstall
any template.


## Usage once installed

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
dotnet new consoleable [--name MyName] [--xunit] [--nunit] [--sln] [--serilog] [--testbase] [--netstandard2]
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
# --xunit : also generate a skeleton xunit test project for the new project
# --nunit : also generate a skeleton nunit test project for the new project
# --sln : also generate a solution file referencing the new project(s).
# --serilog : use Serilog for logging
# --testbase : use TestBase fluent assertions in your tests
# --netstandard2 : generate a netstandard2 classlib instead of a console-runnable net5 executable.
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

#### Long Example

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
dotnet new consoleable --name Freddie --xunit --testbase --sln --serilog ; cd Freddie && dotnet test ; cd Freddie ; dotnet run
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

### Opinions? What Opinions?

These opinions:

-   Anything bigger than ½ a day's work wants testability, instrumentation and
    configurability, but we don't want to write all that boilerplate
    again every. single. time.

-   When run from a commandline, you want an `ILoggerFactory`, an
    `IConfigurationRoot`, and a `Settings` object.

-   Those three things should work straight out of the box when you use the
    template, and then be instantly editable to your own favourite logging,
    configuration and/or settings solution. “Work straight out of the box” means
    that you can `dotnet run` and `dotnet test` immediately after creating the
    new project.

-   The command-line should do some elementary argument parsing and if
    appropriate show help text and immediately exit.

-   These *command-line Concerns* should stay out of your way and be
    ever-so-neatly separated from the Concerns of your component's
    functionality.

### Why is it better than `dotnet new console`?

Because

1. It does the boilerplate for you for logging, configuration, and xunit or nunit tests  

2. It cleanly separates the concerns of Your Component from the concerns of
    being a Self-hosted Command-line Tool, and it does the boilerplate for you for being 
    a command-line tool.

3. You can easily replace the opinions with your own.

### Why is it better than `dotnet new classlib`?

Who doesn't secretly want their components to be independently usable and
testable from the commandline?

(If the answer is you don't, then remove `<OutputType>Exe</OutputType>` from the csproj).

### Why is it worse than `dotnet new classlib`?

It isn't :-) But if you really just wanted a classlib with logging, configuration and testing,
and don't want a command line executable, then remove the `<OutputType>Exe</OutputType>` line 
from the csproj file.

Including `<OutputType>Exe</OutputType>` in the csproj file causes an
additional, executable, copy of your dll to be output, but with 100K of command-line
launcher added. It seems a fair trade-off.

### Comments

Everything related to Self-hosting is in the `SelfHosting/` folder. Your component
itself is in the top-level of the namespace. This seems better than the other way round.

I borrowed the opinion from AspNetCore that a Startup class was a good place to
configure a Logger, ConfigurationRoot and Settings.

Adding my favourite boilerplate (xunit,nunit,serilog,testbase) as commandline
options turned out to be marvellously simple.

Adding your own favourite boilerplate is only a fork & a pull request away.
Unless you have you really obscure minority opinions. In that case, it's only a
fork and no pull request away.

Any Examples?
-------------

https://nuget.org/packages/MailMerge was the somewhat messier forerunner to this
template.

What Opinions did you carefully avoid?
--------------------------------------

You can add your own choice of logging provider and test framework.

What Opinions did you *not* carefully avoid?
------------------------------------------

Dotnet rocks; serilog is great; other logging frameworks are available;
there's nothing to choose between NUnit & xUnit except taste and 15 years of
assertion helpers; tests should express specifications, which is often best
written as WhenXGivenYThenZ; NFRs can be tested; TestBase fluent assertions are
great.
