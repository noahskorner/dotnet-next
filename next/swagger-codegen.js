const { codegen } = require("swagger-axios-codegen");
codegen({
  methodNameMode: "operationId",
  source: require("../dotnet/Api/swagger.json"),
});
