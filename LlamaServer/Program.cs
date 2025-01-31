// Usage:
// --model "DeepSeek-R1-Distill-Llama-8B-Q8_0.gguf" --port 5598 --contextSize 1024 --gpuLayers 34 --maxTokens 256

await LlamaRestServer.Main(args);