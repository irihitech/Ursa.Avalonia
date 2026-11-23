# Third-Party Notices

This repository includes third-party code copied from other projects. This document records provenance, license information, and the list of copied files so downstream users have traceability and can comply with original license obligations.

## Ursa Fluent 2 theme

`src/Ursa.Themes.Fluent2`, `demo/Ursa.Fluent2.Demo`, and
`tests/Ursa.Themes.Fluent2.Tests` were migrated from the author's
`rabbitism/Irihi.Ursa.Themes.Fluent2` draft. Its control templates, converters,
localization resources, and animation helpers were adapted from this
repository's Semi theme at commit
`00fef728ce87cc8cbc9085f3162867daa1d57490`, under this repository's MIT license.

The Fluent 2 resource mappings reference Avalonia's implementation at commit
[`52660000b0ba97a3d2bac13cc43a276708a6d66b`](https://github.com/AvaloniaUI/Avalonia/tree/52660000b0ba97a3d2bac13cc43a276708a6d66b/src/Avalonia.Themes.Fluent2).
Avalonia's implementation is consumed through its NuGet packages, not copied
into this repository. The theme remains experimental while those dependencies
are available only from Avalonia's CI feed.

## Avalonia.Labs — QrCode control

- Upstream project: https://github.com/AvaloniaUI/Avalonia.Labs

- License: MIT
  - Upstream LICENSE (at commit): https://github.com/AvaloniaUI/Avalonia.Labs/blob/a878d4eeec0b8e6c924db2f2c4e631460ccbe67c/LICENSE

```
MIT License

Copyright (c) 2023 AvaloniaUI

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```