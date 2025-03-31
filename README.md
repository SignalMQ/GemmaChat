# GemmaChat
Hi there 👋👋👋

This project allows you to link a running model in your local [LM Studio](https://lmstudio.ai/) with the Telegram Bot API.

## What types of interactions are currently implemented:
- Messages ✅
- Edited messages ❌
- Reactions ❌
- Pictures ❌

I must admit right away that I didn't think about interacting with a bot in a group, so I don't know if it will work.

## Where to start:
1) First, launch LM Studio, download the model.
2) Enable developer mode and launch the server
2) Clone this project locally and open it in [Visual Studio](https://visualstudio.microsoft.com/ru/)
3) Open appsettings.json, there you will see the following parameters:
- `BotToken` - the token used to interact with the Telegram bot interface
- `LLM` - the address to the server, which is shown in LM Studio when switching to the Development tab
- `Sqlite` - the connection string for the local database, usually a path to the `*.db` file
4) There are various configurations in the `Constant.cs` file, they can also be changed depending on the needs. There are parameters such as:
- `Completions` - endpoint for receiving a response from LLM
- `List` - endpoint for receiving a list of models (I do not use it yet)
- `Model` - default model identifier, you can see it in LM Studio in the Development section
- `SystemMessage` - system hint for the model (write there instructions on how the model should respond to the user)

And finally, it is enough for me that you support me in the form of a star ⭐️
