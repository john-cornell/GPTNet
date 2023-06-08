JC.GPT.NET
Welcome to JC.GPT.NET! This is a C# wrapper crafted for language models, specifically focusing on chat models like OpenAI's gpt-3.5-turbo. We aim to assist developers in conveniently integrating these powerful language models into their .NET applications.

PLease understand it is still in its very early stages, however is actively being built and maintained. 

Table of Contents
Introduction
Installation
Usage
Prompt Engineering
Contributing
License
Next Steps
Introduction
JC.GPT.NET is engineered to ease the integration of language models into your C# applications. With the powerful capabilities of OpenAI's gpt-3.5-turbo at your disposal, you can innovate in creating advanced chat-based applications, virtual assistants, and so much more.

Installation
To get started with JC.GPT.NET in your C# project, you have two options - download the source code and compile it yourself or simply install the library through NuGet.

bash
Copy code
# Install via NuGet
dotnet add package JC.GPT.NET
Usage

Please visit [https://github.com/john-cornell/Boots] for a reference implementation project

In the JC.GPT.NET framework, conversations are the centerpiece of interactions with language models. For successful operation, roles such as 'System' and 'Assistant' need to be established. You can define these roles as Role objects, or use RoleBehaviour for more personalized interactions.

Start by importing the GPTEngine and GPTEngine.Roles namespaces in your C# code.

csharp
Copy code
using GPTEngine;
using GPTEngine.Roles;
You can define a conversation by creating specific roles. For example, a SupervisorAgentSystem role and a SupervisorAgentAssistant role can be defined by extending the Role class.

csharp
Copy code
public class SupervisorAgentAssistant : Role
{
    // constructor and behaviour definition
}

public class SupervisorAgentSystem : Role
{
    // constructor and behaviour definition
}
These roles are then added into a Conversation which the GptChatModel can process.

csharp
Copy code
public class Supervisor : Conversation
{
    public Supervisor(string task) : base(new SupervisorAgentSystem(task), new SupervisorAgentAssistant(task), false)
    {

    }
}
Generate a response using the GenerateResponse method of the GptChatModel.

By default, the whole conversation will be sent to GPT, to conserver context, however an additional constructor paramter can be added to instruct the conversation to only send the given message for call.

Additionally, the Conversation will fire a `OnMessageAdded` event whenever a new message is added to it to send, or it receives a respone from the GPT engine.

csharp
Copy code
var response = chatModel.GenerateResponse(conversation);
Console.WriteLine(response);
We're working on introducing a simpler chat engine in future JC.GPT.NET versions. This engine will use pre-set roles to simplify setup and streamline the integration of language models into your applications.

Please refer to the project's GitHub repository for detailed examples on defining roles and their behaviours.

Prompt Engineering
Prompt engineering is all about creating effective prompts to elicit desired responses from language models. JC.GPT.NET allows you to experiment with different prompt engineering strategies to enhance user experiences and customize the model's responses to specific scenarios.

Prompt engineering techniques tailored to JC.GPT.NET are still in active exploration, given the early stage of the library. Keep an eye out for future updates as we fine-tune our approach to optimally utilizing prompt engineering with this library.

Contributing
We warmly welcome contributions to JC.GPT.NET! If you have innovative ideas, bug reports, or feature requests, please don't hesitate to open an issue on the project's GitHub repository. Your feedback and involvement are invaluable to us in enhancing this library.

License
JC.GPT.NET is currently licensed under the GPL-3 License. This choice is intended to discourage the integration of this early-stage software into commercial products. However, as the library evolves, we may shift to a more permissive license. Our primary intention is not to limit the use
