Consoleable
===========

A slightly-opinionated `dotnet new` template for a component classlib which also runs
from the commandline. The opinions are that logging, configuration and testing
are good things, but should each be opt-in and should then work out of the box.

## How to install & uninstall templates from NuGet
```
dotnet new --install Consoleable #finds the Consoleable templates on NuGet
dotnet new consoleable --help
#example: dotnet new consoleable --xunit --sln ; dotnet test ; dotnet run'
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
# --net5 | --netstandard2 : generate a net5/netstandard2 instead of net6.
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
#### Long Example

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
dotnet new consoleable --name Freddie --xunit --testbase --sln --serilog ; cd Freddie && dotnet test ; cd Freddie ; dotnet run
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

#### Gotchas
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
# • For `dotnet test` to work from the top-level directory there has to be a solution file, either
# your own or generated with --sln option. Otherwise, cd into the test project.
# • Using --netstandard2 generates a classlib only, you can't have a netstandard executable.
# • To generate executables for multi-platform, there is a learning curve and there are choices. 
# See e.g. https://docs.microsoft.com/en-us/dotnet/core/deploying/ or https://duckduckgo.com/?q=dotnet+generate+cross+platform+executable  
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


### Opinions? What Opinions?

These opinions:

- Anything bigger than ½ a day's work wants testability, instrumentation and
    configurability, but we don't want to write all that boilerplate
    again every. single. time.

- When run from a commandline, you want help text, an `ILoggerFactory`, an
    `IConfigurationRoot`, and a `Settings` object ready for for you.

- Those things should work straight out of the box when you use the
    template, and then be instantly editable to your own favourite logging,
    configuration and/or settings solution. “Work straight out of the box” means
    that you can `dotnet run` immediately after creating the new project, or
    can `dotnet test` immediately after creating a solution with project &amp; test project.

- The command-line should do some elementary argument parsing and if
    appropriate show help text and immediately exit.

- These *command-line Concerns* should stay out of your way and be
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
1. Who doesn't secretly want their components to be independently usable and
testable from the commandline?
   - _(If your answer is ‘Me’, then remove `<OutputType>Exe</OutputType>` from the csproj)._

### Why is it worse than `dotnet new classlib`?
1. It isn't :-)
2. But if you really just wanted a classlib with logging, configuration and testing,
and don't want a command line executable, then remove the `<OutputType>Exe</OutputType>` line 
from the csproj file.
3. Including `<OutputType>Exe</OutputType>` in the csproj file causes an
additional, executable, copy of your dll to be output, but with 100K of command-line
launcher added. It seems a fair trade-off especially since it still generates the just-your-component dll too.

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

- Dotnet is great
- Serilog is great; other logging frameworks are available
- There's nothing to choose between NUnit & xUnit except taste and 15 years of
  assertion helpers
- Tests should express specifications, and WhenXGivenYThenZ is often a helpful format
- Some NFRs can be unit-tested
- TestBase fluent assertions are great