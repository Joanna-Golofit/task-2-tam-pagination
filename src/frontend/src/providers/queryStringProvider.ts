const flatten = (object: any, prefix?: string, currentResult = {}) => Object.entries(object)
  .reduce((result: any, [key, value]) => {
    const obj = result;
    const propertyName = prefix ? `${prefix}.${key}` : key;
    if (Array.isArray(value)) {
      obj[propertyName] = value.join(',');
    } else if (typeof value === 'object') {
      flatten(value, propertyName, obj);
    } else {
      obj[propertyName] = value;
    }

    return obj;
  }, currentResult);

const queryString = (object: any) => Object.entries<any>(flatten(object))
  .filter(([, value]: string[]) => value?.toString().length > 0)
  .reduce((query: string, [key, value]: string[]) => `${query}${key}=${encodeURIComponent(value)}&`, '?')
  .slice(0, -1);

export { queryString };
