# Yafn

Simple parser of binaries for *Languages and paradigms* task

# Example usage

```C#
public static void Main(string[] args) {
  ModuleLayout layout = ModuleLayout.ReadModuleLayout("some_binary.ptptb");
  Module module = new Module(layout);
  
  foreach (Section s in module.Sections) {
    Console.WriteLine($"Found section {s.Name} with {s.Labels.Length} labels");
  }
}
```
