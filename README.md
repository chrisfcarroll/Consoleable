# Consoleable 

A slightly-opinionated `dotnet new` for a component which may be used from the commandline or as an class library

### Opinions! What Opinions?

- When run from a commandline, the `SelfHosting.Startup` class should initialise a LoggerFactory, a Configuration root, and a Settings object. 
  Those three things should work straight out of the box when you use the template, but should be instantly editable to your favourite logging, 
  configuration and/or settings solution.
- The command-line should do some elementary argument parsing and show help text and immediately exit if appropriate.
- These command-line `Concerns` should be neatly separated from the `Concerns` of your component's functionality. 

### Why is it better than `dotnet new console`?

Because
1. It cleanly separates the concerns of your Component from the concerns of being a self-hosted command-line tool
2. It addresses the concerns of being a command-line tool for you, out of the box, saving you the grunt work.
3. You can easily replace the opinions with your own.

## What Opionions did you carefully avoid?

Add your own choice of logging provider, test unit framework.
