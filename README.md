# llama.cpp REST Server

A rest server that uses LLAMASharp to provide a REST API to generate text from a `.gguf` file.

# Build

- Visual Studio 2022
- Currently has ``LLAMASharp.Backend.Cuda12`` as the backend, change it out if you would rather have `Cuda11` or `CPU`.

# Download

- [Download the latest release](https://github.com/vltmedia/LlamaServer/releases)

# Models

`.gguf` files are required to run the server. These files can be found on the [HuggingFace](https://huggingface.co/models?library=gguf&sort=trending/) website.

# Usage

```bash
LlamaServer.exe --model "P:\CodeDump\DeepseekTest\DeepSeek-R1-GGU_1Q1S\DeepSeek-R1-Distill-Llama-8B-Q8_0.gguf" --port 5598 --contextSize 1024 --gpuLayers 34 --maxTokens 256
```

# Options

| Option            | Required | Default                                      | Description                        |
| ----------------- | -------- | -------------------------------------------- | ---------------------------------- |
| `--model`       | ✅ Yes   | `"DeepSeek-R1-Distill-Llama-8B-Q8_0.gguf"` | Path to the GGUF model file.       |
| `--port`        | ❌ No    | `5598`                                     | Port number for the REST API.      |
| `--contextSize` | ❌ No    | `1024`                                     | Context size for Llama model.      |
| `--gpuLayers`   | ❌ No    | `34`                                       | Number of GPU layers to offload.   |
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

## Antiprompts

Antiprompts are a way to keep the AI from generating infinitly. Build these strings into your system message to keep the AI from generating too much text.

```csharp
AntiPrompts = new List<string> { "}\n", "} ", "<|END|>", "<|END|>\n", "<|FINISHED|><|END|>\n" }
```
