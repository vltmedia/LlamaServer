# llama.cpp REST Server

A rest server that uses LLAMASharp to provide a REST API to generate text from a `.gguf` file.

# Build

- Visual Studio 2022
- Currently has ``LLAMASharp.Backend.Cuda12`` as the backend, change it out if you would rather have `Cuda11` or `CPU`.


# Solution Applications
## LlamaServer.exe
The main application that runs the REST server.

## LlamaServer_Installer.msi
An installer for the server. It will install the server to `C:\Program Files\Llama Server\Llama Server`, and add an icon to your Start Menu.

# Download

- [Download the latest release](https://github.com/vltmedia/LlamaServer/releases)

# Models

`.gguf` files are required to run the server. These files can be found on the [HuggingFace](https://huggingface.co/models?library=gguf&sort=trending/) website.

## Auto Loading Model
If you leave the `--model` option blank, the server will attempt to load the default model from the `config.json` file. If the file is not found or it's empty, the application will check if any `.gguf` files are in the `models` directory and load the first one it finds.

## Model Config
A model config file is available to aid in the default loading of a model at `Llama Server/models/config.json`. Just update the `DefaultModel` field to the path of your model.

```json
{
  "DefaultModel": "DeepSeek-R1-Distill-Llama-8B-Q8_0.gguf"
}
```


# Usage

## Start the Server

Change the model path, and the port number to your desired values, and any other options you would like to change.

```bash
LlamaServer.exe --model "DeepSeek-R1-Distill-Llama-8B-Q8_0.gguf" --port 5598 --contextSize 1024 --gpuLayers 34 --maxTokens 256 --temperature 0.5 --seed 52 --returnJson true
```

Currently the app is made to return a JSON string, if you would like to change this [Go to Changing Behavior](#changing-behavior).

## Send a Request

```bash
curl -X POST "http://localhost:5598/generate" -H "Content-Type: application/json" -d "{\"UserInput\":\"Hi, my name is John Smith and I work at Place as a worker.\"}"
```

### Response

```json
{
  "headline": "Worker's Vision Transforms Industry",
  "body": "John Smith's innovative approach at Place is revolutionizing the field."
}
```

# Options

| Option            | Required | Default                                      | Description                        |
| ----------------- | -------- | -------------------------------------------- | ---------------------------------- |
| `--model`       | ✅ Yes   | `"DeepSeek-R1-Distill-Llama-8B-Q8_0.gguf"` | Path to the GGUF model file.       |
| `--port`        | ❌ No    | `5598`                                     | Port number for the REST API.      |
| `--contextSize` | ❌ No    | `1024`                                     | Context size for Llama model.      |
| `--gpuLayers`   | ❌ No    | `34`                                       | Number of GPU layers to offload.   |
| `--seed`        | ❌ No    | `42`                                      | Seed used to change generated media.       |
| `--temperature` | ❌ No    | `0.5`                                      | Temperature for the model.       |
| `--maxTokens`   | ❌ No    | `256`                                      | Maximum tokens for response.       |
| `--returnJson`  | ❌ No    | `true`                                     | Return a JSON string for response. |

# Customizing

## Changing Behavior

To replace the base behavior of the ai, change the `.system` message that we pass into the model:

```csharp
var chatHistory = new ChatHistory();
        chatHistory.AddMessage(AuthorRole.System,
    "You are a pulitzer prize headline writer. Write a Headline and Body text for a Magazine front-page headline about the person in the data provided. Make the generated JSON string about them and the answers provided. \n"
    + "Incorporate the person's name, and company, artistically allude to the person's title, and answers.  \n"
    + "Headline should be a maximum of 8 words, and Body should be a maximum of 20 words. \n"
    + "You must generate a JSON string in this format: \n"
    + "<|START|>{ \"headline\": \"Your headline here\", \"body\": \"Your body text here\" }<|FINISHED|><|END|>\n"
    + "You MUST ONLY return a valid JSON string. DO NOT provide explanations. DO NOT add extra text. \n"
    + "When you are finished, respond with <|END|>. \n"
    + "Example:\n"
    + "Output:\n"
    + "<|START|>{ \"headline\": \"AI Breakthrough Redefines Business\", \"body\": \"Industry experts say this changes everything.\" }<|FINISHED|><|END|> \n"
    + "Now generate a response in the same format. Begin output:\n"
);
```
### DeepSeek
If you are using the DeepSeek models, especially Q2-Q6 versions, you will have to be explicit with the system message or it will start second guessing itself and ramble on and on.

## Antiprompts

Antiprompts are a way to keep the AI from generating infinitly. Build these strings into your system message to keep the AI from generating too much text.

```csharp
AntiPrompts = new List<string> { "}\n", "} ", "<|END|>", "<|END|>\n", "<|FINISHED|><|END|>\n" }
```
