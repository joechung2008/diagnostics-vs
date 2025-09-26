DiagnosticsVS.Tailwind

This project builds Tailwind CSS using npm. To build the CSS:

1. Install Node.js.
2. From the project directory, run `npm ci`.
3. Run `npm run build:css` to produce `dist/tailwind.css`.

To integrate with the .NET build, pass `-p:RunTailwindBuild=true` when building this project.
