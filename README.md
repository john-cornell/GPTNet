# GPTNet
Still very early on, will update

Usage: Create a Role or RoleBehaviour and pass to a Conversation, then send to a GPT instance

I have created https://github.com/john-cornell/Boots/tree/master as an example project for usage, it'll have to serve as documentation until I get around to writing some, I'll try to keep it up to date with changes. Also see https://github.com/john-cornell/GPTEngine for examples, but not guaranteeing this won't diverge from that project, it was just a silly first cut project.

Package available from Nuget at JC.GPT.NET

Currently license is GPL-3, largely to stop people being stupid enough to include very early software in commercial products. When it is more advanced, I will switch it to something more open. I have no desire to limit its use, just don't want to cause anyone pain.

Next Up:
Add support for other LLMs
Token counting and summarisation of older messages
Investigate dependency injection frameworks to handle other LLMs
