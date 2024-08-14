const { writeFile } = require('fs');
require('dotenv').config();

const targetPath = `./src/environments/environment.ts`; // Use "environment.ts" para dev, se necessário.

const envConfigFile = `
export const environment = {
   production: true,
   api_url: "${process.env.API_URL}"
};
`;

writeFile(targetPath, envConfigFile, function (err) {
   if (err) {
      console.log(err);
   }
   console.log(`Environment file generated at ${targetPath}`);
});
