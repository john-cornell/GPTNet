# GPT.Net - Simple GPT Chatbot
## Introduction

GPT.Net is a C# wrapper for Large Language Model APIs, currently supporting OpenAI GPT models, Anthropic Claude models and Huggingface models via the Huggingface inference endpoint. This repository will be kept up to date, and contributions are welcome.

## Components 

The library currently employs the following main components:

### Simple GPTChat bot

The GPTChat class provides a simple interface for chatting with GPT models from OpenAI, Anthropic or Huggingface. You instantiate it by passing the required API keys and model names, then call Chat() to send a message and get a response.

### GPT instances & Conversations
GPT instances are created using the GPTApiFactory, by requesting either a GPTApiOpenAI (for OpenAI models), GPTApiAnthropic (for Anthropic Claude models) or GPTApiHuggingface (for Huggingface inference endpoint models) instance.

These GPT instances can then generate Conversations, which store the conversation state/history and configuration like temperature. Messages are added to the Conversation, which is then passed to the GPT instance to get a response.

Conversations describe the initial prompts for the conversation, and also handle assigning Roles like System, Assistant and User. Conversations can be configured to reset/clear the conversation history on each message, or maintain the full history.

### Events
GPTChat and Conversations both expose events for:

* OnMessage (or OnMessageAdded for Conversations) 
    * When a message is sent/received, useful for logging conversations

* OnError 
    * If there is an error calling the API, so it can be gracefully handled

## API

To get started, install the NuGet package GPTNet. Then:
```csharp
using GPTNet;  
```

## Settings

To protect API keys it is recommended to store these in a config file such as appsettings.json rather than store them directly in code.

```json
{
  "ApiKey": "your-openai-key",
  "Model": "curie",  
  "HFAPIKey": "your-huggingface-key",     
  "HFModel": "distilbert-base-cased"  
}     
```

These can then be simply retrieved, and used in code like as follows:

```csharp
IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

GPTChat chat = new GPTChat(configuration["ApiKey"], configuration["35Model"]);
```

## GPTApiProperties
`ChatGPT` ctors and `GPTApiFactory` calls now recommend passing a `GPTApiProperties` class to hold details, rather than send properties down as individual parameters. This has been done to simplify things as the Anthropic model required a modelVersion property, and I wanted to avoid parameter blow out. 

Please use these methods and ctors where available, as I won't necessarily be updating the others if new parameters are required for later models, or I add top_p and top_k and other finetuning.

(configuration keys given in the code below are advisory only, it's up to you to define your own, or hardcode data in directly - though this is not recommended for keys if you are planning on using any sort of source control)

```csharp
//OpenAI
//This now uses GPTApiOpenAI class. 
GPTApiProperties properties = GPTApiProperties.Create<GPTApiOpenAI>(
                configuration["ApiKey"], configuration["Model"]); 
                
//Anthropic
//modelVersion is used for the Anthropic agent, but currently defaults to "2023-06-01", and temperature may also be passed
GPTApiProperties properties = GPTApiProperties.Create<GPTApiAnthropic>(
                configuration["ApiKey"], configuration["Model"], configuration["ModelVersion"]); 
                
//Huggingface
GPTApiProperties properties = GPTApiProperties.Create<GPTApiHuggingface>(
                configuration["ApiKey"], configuration["Model"]); //and temperature may also be passed

```

Note: `GPTOpenAI` still exists for backwards compatability reasons, but renamed a new class to keep to standards. It is currently identical to the new `GPTApiOpenAI` but it is recommended to use `GPTApiOpenAI` in future, as I can't guarentee I will remember to always update GPTOpenAI (I will do my best) for later changes

## GPTChat
The GPTChat class provides a simple bot interface. Initialize with your OpenAI or Huggingface info:

```csharp
//Get Chat
GPTChat chat = new GPTChat(properties)
```

The `GPTApiProperties` object has now obseleted the following ctors, which will still work for backwards compatibility, however I can't guarentee I will remember to always update this (I will do my best) for later models
```csharp
// OpenAI       
GPTChat chat = new GPTChat(Configuration["ApiKey"], Configuration["Model"]);        

// Huggingface   
chat = new GPTChat(Configuration["HFApiKey"], Configuration["HFModel"], GPTApiType.Huggingface);  

//Anthropic
//Use the properties ctor
```

## GPTChat exposes events:

* OnMessage - Fires when a message is sent/received. Use to log the conversation.
* OnError - Fires if there is an error calling the API. Use to handle errors gracefully.
Then call Chat() and get a response:

```csharp
var response = await chat.Chat("Hello! How are you?");        
Console.WriteLine(response.Response);        
// Prints "I'm doing well, thanks for asking!"   
```

## Advanced - GPTApi, Conversations and ConversationFactory

For more control, use the GPTApiFactory to generate an API instance, then generate a Conversation from that to communicate. Conversations can be configured to remove history on each message, though keeps it by default.

The `GPTChat` and `GPTChatTests` classes may be used as examples of working code, and will be kept updated as they are the test harnesses I use to ensure everything runs fine

```
_gptApi = new GPTApiFactory().GetApi(properties);            
```
The `GPTApiProperties` object has now obseleted the following, which will still work for backwards compatibility, however I can't guarentee I will remember to always update this (I will do my best) for later models:

```csharp
IGPTApi openai = new GPTApiFactory().GetApi<GPTOpenAI>(Configuration["ApiKey"], Configuration["Model"]);//HttpClient (mainly used for tests), modelVersion (Anthropic requirement) and temperature can also be optionally passed
 var conversation = openai.GenerateConversation(false); 
```
### Conversation events and settings
* `OnMessageAdded` event
* `Temperature` (0-1) to control randomness
* `MaxTokens` for longer responses.

## Future steps
* Prompt templates and types (build from data, chain of thought, tree of thought)
* New OpenAI functions

For questions or to contribute, email john at johncornell dot org
