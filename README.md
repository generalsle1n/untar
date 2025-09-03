# Untar

Untar is a simple and efficient tool for extracting `.tar`, `.tar.gz`, and `.zip` archive files. It is built using .NET 9 and provides a command-line interface for easy usage.

## Features

- Supports `.tar`, `.tar.gz`, and `.zip` archive formats.
- Automatically detects the archive type.
- Extracts files and directories while maintaining the original structure.
- Lightweight and easy to use.

## Prerequisites

- .NET 9 SDK installed on your system.

## Installation

1. Clone the repository:git clone https://github.com/yourusername/untar.git2. Navigate to the project directory:cd untar3. Build the project:dotnet build
## Usage

Run the tool from the command line:untar <file>
### Example
To extract a `sample.tar.gz` file:untar sample.tar.gz
The extracted files will be placed in a folder named after the archive file.

## Project Structure

- `Archive/` - Contains implementations for handling different archive types (`Tar`, `TarGz`, `Zip`).
- `Model/` - Contains shared models and enums.
- `FileTypeCheck.cs` - Logic for detecting the archive type.
- `Program.cs` - Entry point of the application.

## Contributing

Contributions are welcome! Feel free to open issues or submit pull requests.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.