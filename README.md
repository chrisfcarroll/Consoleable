# Consoleable 

A slightly-opinionated `dotnet new` template for a component which may be used from the commandline or as an class library

Usage:

```
dotnet new consoleable [--name MyName] [--xunit] [--nunit] 
```
```
#use the --xunit or --nunit flags to get a skeleton xunit/nunit test project
#dotnet run the project directory and/or dotnet test the test projects as usual to build and run
```

### Opinions?! What Opinions?

These opinions:
- When run from a commandline, the `SelfHosting.Startup` class should initialise a `LoggerFactory`, a `Configuration` root, 
  and a `Settings` object. 
- Those three things should work straight out of the box when you use the template, and then be 
  instantly editable to your own favourite logging, configuration and/or settings solution.
- The command-line should do some elementary argument parsing and if appropriate show help text and immediately exit.
- These command-line `Concerns` should be oh-so-neatly separated from the `Concerns` of your component's functionality. 

### Why is it better than `dotnet new console`?

Because
1. It cleanly separates the concerns of your Component from the concerns of being a self-hosted command-line tool
2. It addresses the concerns of being a command-line tool for you, out of the box, saving you the grunt work.
3. You can easily replace the opinions with your own.

## What Opionions did you carefully avoid?

Add your own choice of logging provider and test framework.


### Adding (to a) Solution
If you use the template in an empty directory, then 
```
dotnet new sln && dotnet sln add Relay Relay.Specx && dotnet test
```
will create a new solution, add project and tests, and run the tests
