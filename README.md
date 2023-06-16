# GPTNet - Simple GPT Chatbot
##Introduction

C# wrapper for Large Language Models, currently OpenAI and Huggingface, with Anthropic Claude coming soon. This repository will be kept up to date, and contributions are welcome.

##Components 

The library currently employs the following main components:

###Simple GPTChat bot

The GPTChat class provides a simple interface for chatting with GPT models from OpenAI or Huggingface. You instantiate it by passing the required API keys and model names, then call Chat() to send a message and get a response.

###GPT instances & Conversations
GPT instances are created using the GPTApiFactory, by requesting either a GPTOpenAI (for OpenAI models) or GPTApiHuggingface (for Huggingface models) instance.

These GPT instances can then generate Conversations, which store the conversation state/history and configuration like temperature. Messages are added to the Conversation, which is then passed to the GPT instance to get a response.

Conversations describe the initial prompts for the conversation, and also handle assigning Roles like System, Assistant and User. Conversations can be configured to reset/clear the conversation history on each message, or maintain the full history.

###Events
GPTChat and Conversations both expose events for:

* OnMessage (or OnMessageAdded for Converstaions) 
    * When a message is sent/received, useful for logging conversations
OnError 
    * If there is an error calling the API, so it can be gracefully handled

##API
To get started, install the NuGet package GPTNet. Then:
```
using GPTNet;  
```

##Settings
Use appsettings.json to store API keys and models:

```
{
  "ApiKey": "your-openai-key",
  "Model": "curie",  
  "HFAPIKey": "your-huggingface-key",     
  "HFModel": "distilbert-base-cased"  
}     
```

##GPTChat
The GPTChat class provides a simple bot interface. Initialize with your OpenAI or Huggingface info:

```
// OpenAI       
GPTChat chat = new GPTChat(Configuration["ApiKey"], Configuration["Model"]);        

// Huggingface   
chat = new GPTChat(Configuration["HFApiKey"], Configuration["HFModel"], GPTApiType.Huggingface);   
```

##GPTChat exposes events:

* OnMessage - Fires when a message is sent/received. Use to log the conversation.
* OnError - Fires if there is an error calling the API. Use to handle errors gracefully.
Then call Chat() and get a response:

```
var response = await chat.Chat("Hello! How are you?");        
Console.WriteLine(response.Response);        
// Prints "I'm doing well, thanks for asking!"   
```

##Advanced - GPTApi, Conversations and ConversationFactory

For more control, use the GPTApiFactory to generate an API instance, then generate a Conversation from that to communicate. Conversations can be configured to remove history on each message, though keeps it by default.
```
IGPTApi openai = new GPTApiFactory().GetApi<GPTOpenAI>(Configuration["ApiKey"], Configuration["Model"]);
 var conversation = openai.GenerateConversation(false); 
```

Conversations expose 
* `OnMessageAdded` and allow setting `Temperature` (0-1) to control randomness and `MaxTokens` for longer responses.

##Future steps
Prompt templates and types (build from data, chain of thought, tree of thought)
Claude integration
New OpenAI functions
For questions or to contribute, email john at johncornell dot org
