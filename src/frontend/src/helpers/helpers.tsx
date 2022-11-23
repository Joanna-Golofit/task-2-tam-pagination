export { arrIsNullOrEmpty, stringFormat, strIsNullOrEmpty };

function stringFormat(template: string, ...args: string[]): string {
  if (!template.match(/^(?:(?:(?:[^{}]|(?:\{\{)|(?:\}\}))+)|(?:\{[0-9]+\}))+$/)) {
    throw new Error('invalid format string.');
  }
  return template.replace(/((?:[^{}]|(?:\{\{)|(?:\}\}))+)|(?:\{([0-9]+)\})/g, (m, str, index) => {
    if (str) {
      return str.replace(/(?:{{)|(?:}})/g, (s: string) => s[0]);
    }
    if (index >= args.length) {
      throw new Error('argument index is out of range in format');
    }
    return args[index];
  });
}

function strIsNullOrEmpty(str: string): boolean {
  return (!str || str.length === 0);
}

function arrIsNullOrEmpty(arr: any[]): boolean {
  return (!arr || arr.length === 0);
}
