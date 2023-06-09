# JC.GPT.NET

Welcome to JC.GPT.NET! 

This is a C# wrapper crafted for language models, at present specifically focusing on OpenAI's chat models, such as `gpt-3.5-turbo`, where specific System, Assistant and User roles are expected, however I plan to extend this to other models and model types in the near future. 

As fun as python is to code in, the aim of GPT.NET to assist developers in conveniently integrating powerful language models into their .NET applications. 

Please understand it is still in its very early stages, however, both the codebase and this documentation actively being built and maintained. 

Please feel free to contribute any way you feel you wish.

## Table of Contents
- [Introduction](#introduction)
- [Installation](#installation)
- [Usage](#usage)
- [Prompt Engineering](#prompt-engineering)
- [Contributing](#contributing)
- [License](#license)
- [Next Steps](#next-steps)

## Introduction
JC.GPT.NET is engineered to ease the integration of language models into your C# applications, to take the tedious integration steps out of the way, you can innovate in creating advanced chat-based applications, virtual assistants, and so much more.

## Installation
To get started with JC.GPT.NET in your C# project, you have two options - download the source code and compile it yourself or simply install the library through NuGet.

```bash
# Install via NuGet
Install-Package JC.GPT.NET

#or
dotnet add package JC.GPT.NET
```

## Usage

Please visit [Boots](https://github.com/john-cornell/Boots) for a reference implementation project that I will endeavour to keep up to date

Alternatively [GPTAssessorEngine](https://github.com/john-cornell/GPTAssessorEngine) is another fun implementation, using prototype code that was later used in the core of this project, to create a lying lexicographer as a test. Get it to define any nonesense word you wish. Fun for all the family. *That* project, however, is not getting maintained, and was a fairly quick POC so expect it to diverge from this project very quickly. 

In the JC.GPT.NET framework, conversations are the centerpiece of interactions with language models. For successful operation, roles such as 'System' and 'Assistant' need to be established. You can define these roles as Role objects, or use RoleBehaviour to combine the two into a single Role.

Start by importing the GPTEngine and GPTEngine.Roles namespaces in your C# code.

```csharp
using GPTEngine;
using GPTEngine.Roles;
```

As mentioned above, you can define a conversation by creating specific roles. A conversation currently requires 2 roles to be defined, System and Assistant. This reflects the requirements of the OpenAI chat models. 

The System will define to goal of the application you are trying to build, while the assistant describes the goals and means of interaction between the LLM and the user. Generally a system role will be a simplified version of the assistant role.

Using the [Boots](https://github.com/john-cornell/Boots) example, we can see how a conversation is created for a developer agent's interactions with the model. A system and an assistant role are defined and passed to the developer conversation. Here the `Conversation` object is being used as a base class, however that is not required if you prefer to us Conversation on its own. This was only done to contain the developer Roles in one place.

```csharp
public class Developer : Conversation
{
    public Developer(string task) : base(new DeveloperSystem(task),
        new DeveloperAssistant(task), false)
    {

    }
}

public class DeveloperSystem : Role
{
    public DeveloperSystem(string task) : base(RoleType.System, new CustomRoleBehaviour(
        @$"AI Agent, your task is to generate high-quality, efficient C# code to accomplish the following: {task}"))
    {
    }
}

public class DeveloperAssistant : Role
{
    public DeveloperAssistant(string task) : base(RoleType.Assistant, new CustomRoleBehaviour(
        @$"AI Agent, your task is to generate high-quality, efficient C# code to accomplish the following: {task}

Your code should be clear, concise, and fully commented, in accordance with best practices for C# programming. Please ensure to handle any exceptions that may occur and consider edge cases to ensure the robustness of your solution. Include appropriate error handling and logging mechanisms as necessary. Your program should be as modular and reusable as possible.

If you could also provide a brief explanation of your approach and any trade-offs you made, it would be greatly appreciated."))
    {
    }
}
```

Alternatively the RoleBehaviour object can be used to simplify this process into a single prompt that will be used for both System and Agent

Generate a response using the GenerateResponse method of the Gpt model.

```csharp
IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

_gpt = new GPT(configuration["OpenApiKey"], configuration["Model"]);
         
Developer developer = new Developer("Build a method List of FeeEntities and extracts out 2 HashSet of FeeIds, one that has the FeeEntity.Id of Fees already included in the _cache.Fees Dictionary (Fee Id, FeeEntity), and one of FeeEntity.Ids not in cache" );         
         
var developerResponse = await _gpt.Call(developer);
                
if (IsError(developerResponse)) return;

string developerResponseText = developerResponse.Response;
... AND SO ON ...
```

By default, the whole conversation will be sent to GPT, to conserve context, however an additional constructor parameter can be added to instruct the conversation to only send the given message for the call.

Additionally, the Conversation will fire an `OnMessageAdded` event whenever a new message is added to send, or it receives a response from the GPT engine.

```csharp
    developer.OnMessageAdded += (sender, args) => { Application.Current.Dispatcher.Invoke(() => { History.Add(BuildGPTMessageFromEvent(args)); }); };
    supervisor.OnMessageAdded += (sender, args) => { Application.Current.Dispatcher.Invoke(() => { History.Add(BuildGPTMessageFromEvent(args)); }); };```
```

Please refer to the project's GitHub repository for detailed examples on defining roles and their behaviours.

## Prompt Engineering
Prompt engineering is all about creating effective prompts to elicit desired responses from language models. JC.GPT.NET allows you to experiment with different prompt engineering strategies to enhance user experiences and customize the model's responses to specific scenarios.

Prompt engineering techniques tailored to JC.GPT.NET are still in active exploration, given the early stage of the library. Keep an eye out for future updates as I fine-tune optimally utilizing prompt engineering in this library.

## Contributing
I will happily welcome contributions. If you have innovative ideas, bug reports, or feature requests, please don't hesitate to open an issue on the project's GitHub repository. Your feedback and involvement are invaluable to us in enhancing this library.

## License
JC.GPT.NET is currently licensed under the GPL-3 License. This choice is intended to discourage the integration of this early-stage software into commercial products. However, as the library evolves, we may shift to a more permissive license. Our primary intention is not to limit the use.
