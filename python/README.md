<h1 align="center">Retranslator</h1>

 Installation: ```pip install --upgrade retranslator```

 Import:
```python
from retranslator import *
```
Usage with CSharpToCppTranslator:
```python
import retranslator, cs2cpp 
retranslator.TranslatorCLI(args=["test.cs"], translator=cs2cpp.CSharpToCpp, extension=".cpp", useRegex=1)
```
