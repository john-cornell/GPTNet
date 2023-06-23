# GPTNet - Simple GPT Chatbot
## Introduction

C# wrapper for Large Language Models, currently OpenAI and Huggingface, with Anthropic Claude coming soon. This repository will be kept up to date, and contributions are welcome.

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

* OnMessage (or OnMessageAdded for Converstaions) 
    * When a message is sent/received, useful for logging conversations
OnError 
    * If there is an error calling the API, so it can be gracefully handled

## API

To get started, install the NuGet package GPTNet. Then:
```csharp
using GPTNet;  
```

##Settings

Use appsettings.json to store API keys and models:

```json
{
  "ApiKey": "your-openai-key",
  "Model": "curie",  
  "HFAPIKey": "your-huggingface-key",     
  "HFModel": "distilbert-base-cased"  
}     
```

## GPTApiProperties
`ChatGPT` ctors and `GPTApiFactory` calls now recommend passing a `GPTApiProperties` class to hold details, rather than send properties down as individual parameters. This has been done to simplify things as the Anthropic model required a modelVersion property, and I wanted to avoid paramter blow out. 

Please use these methods and ctors where available, as I won't necessarily be updating the others if new parameters are required for later models, or I add top_p and top_k and other finetuning.

```csharp
//OpenAI - Given OpenAI data is in appsettings.json as with these keys.
//This now uses GPTApiOpenAI class. 
//GPTOpenAI still exists for backwards compat, but renamed a new class to keep to standards. 
//Please use GPTApiOpenAI as  I can't guarentee I will remember to always update GPTOpenAI (I will do my best) for later changes
GPTApiProperties properties = GPTApiProperties.Create<GPTApiOpenAI>(
                configuration["ApiKey"], configuration["Model"]); 
                
//Anthropic - Given Anthropic data is in appsettings.json as with these keys. 
//modelVersion is used for the Anthropic agent, but currently defaults to "2023-06-01", and temperature may also be passed

GPTApiProperties properties = GPTApiProperties.Create<GPTApiAnthropic>(
                configuration["ApiKey"], configuration["Model"], configuration["ModelVersion"]); 
//Huggingface - Given Huggingface data is in appsettings.json as with these keys.
GPTApiProperties properties = GPTApiProperties.Create<GPTApiHuggingface>(
                configuration["ApiKey"], configuration["Model"]); //and temperature may also be passed

```

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

The `GPTChat` and `GPTChatTests` may be used as examples of working code, and will be kept updated as they are the test harnesses I use to ensure everything runs fine

```
_gptApi = new GPTApiFactory().GetApi(properties);            
```
The `GPTApiProperties` object has now obseleted the following, which will still work for backwards compatibility, however I can't guarentee I will remember to always update this (I will do my best) for later models:

```csharp
IGPTApi openai = new GPTApiFactory().GetApi<GPTOpenAI>(Configuration["ApiKey"], Configuration["Model"]);//HttpClient (mainly used for tests), modelVersion (Anthropic requirement) and temperature can also be optionally passed
 var conversation = openai.GenerateConversation(false); 
```

Conversations expose 
* `OnMessageAdded` and allow setting `Temperature` (0-1) to control randomness and `MaxTokens` for longer responses.

## Future steps
* Prompt templates and types (build from data, chain of thought, tree of thought)
* New OpenAI functions

For questions or to contribute, email john at johncornell dot org
