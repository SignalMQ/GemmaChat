# GemmaChat
Hi there 👋👋👋

This project allows you to link a running model in your local [LM Studio](https://lmstudio.ai/) with the Telegram Bot API.

## What types of interactions are currently implemented:
- Messages ✅
- Pictures ✅
- Edited messages ❌
- Reactions ❌

## Where to start:
1) First, launch LM Studio, download the model.
2) Enable developer mode and launch the server
3) Clone this project locally and open it in [Visual Studio](https://visualstudio.microsoft.com/ru/)
4) Open appsettings.json, there you will see the following parameters:
- `BotToken` - the token used to interact with the Telegram bot interface
- `LLM` - the address to the server, which is shown in LM Studio when switching to the Development tab
- `Sqlite` - the connection string for the local database, usually a path to the `*.db` file (created automatically)
5) There are various configurations in the `Constant.cs` file, they can also be changed depending on the needs. There are parameters such as:
- `Completions` - endpoint for receiving a response from LLM
- `List` - endpoint for receiving a list of models (I do not use it yet)
- `Model` - default model identifier, you can see it in LM Studio in the Development section
- `SystemMessage` - system hint for the model (write there instructions on how the model should respond to the user)

And finally, it is enough for me that you support me in the form of a star ⭐️
