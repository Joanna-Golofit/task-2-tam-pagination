import dayjs from 'dayjs';

const shortDate = (date: dayjs.ConfigType) => dayjs(date).format('DD-MM-YYYY');

export { shortDate };
