import { useRef, useState } from "react";

interface TextFieldProps {
  label?: string;
  placeholder?: string;
  value?: string | number;
  onInput?: Function;
  onFocus?: Function;
  onBlur?: Function;
  type?: "text" | "password";
}

const TextField = ({
  value,
  onInput = () => {},
  onFocus = () => {},
  onBlur = () => {},
  type = "text",
  label,
  placeholder,
}: TextFieldProps) => {
  const textFieldRef = useRef<any>(null);
  const [showPassword, setShowPassword] = useState(false);
  const [isFocused, setIsFocused] = useState(false);

  const handleOnFocus = () => {
    setIsFocused(true);
    onFocus();
  };
  const handleOnBlur = () => {
    setIsFocused(false);
    onBlur();
  };

  return (
    <div className="relative w-full space-y-1 text-sm">
      {label && (
        <label
          htmlFor=""
          className="text-xs font-semibold uppercase text-slate-300"
        >
          {label}
        </label>
      )}
      <div className="flex w-full items-center  justify-center rounded-md border border-slate-900 shadow-sm">
        <div className="flex w-full items-center justify-between">
          <input
            ref={textFieldRef}
            type={type !== "password" ? type : showPassword ? "text" : type}
            onInput={(e) => onInput((e.target as HTMLTextAreaElement).value)}
            onFocus={handleOnFocus}
            onBlur={handleOnBlur}
            value={value}
            className="w-full rounded-md p-2"
            placeholder={placeholder && placeholder}
          />
        </div>
      </div>
    </div>
  );
};

export default TextField;
