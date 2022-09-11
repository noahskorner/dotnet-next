import TextField from "../../../components/inputs/text-field";

const CreateUserPage = () => {
  return (
    <div className="w-full h-full bg-indigo-800 p-2 center">
      <div className="w-full bg-white rounded-sm p-4 text-center space-y-6">
        <h1 className="text-4xl font-bold">Sign Up</h1>
        <TextField />
      </div>
    </div>
  );
};

export default CreateUserPage;
