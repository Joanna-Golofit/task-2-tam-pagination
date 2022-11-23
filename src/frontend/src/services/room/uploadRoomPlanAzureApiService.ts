import { BlobServiceClient, ContainerClient } from '@azure/storage-blob';
import { RoomForProjectDto } from '../project/models';
import { RoomDetailsDto } from './models';

const containerName = 'room-plans';
const storageAccountName = process.env.REACT_APP_TAM_AZURE_STORAGE_NAME;
const accountStorageUrl = `https://${storageAccountName}.blob.core.windows.net`;

export const uploadRoomPlanAzureApiService = async (roomItem: RoomDetailsDto | RoomForProjectDto, file: File | null) => {
  if (!file) return;
  const sasToken = roomItem.sasTokenForRoomPlans;
  const blobService = new BlobServiceClient(`${accountStorageUrl}?${sasToken}`);
  const containerClient: ContainerClient = blobService.getContainerClient(containerName);
  const blobClient = containerClient.getBlockBlobClient(generateFilename(roomItem));
  const options = { blobHTTPHeaders: { blobContentType: file.type } };

  await blobClient.uploadData(file, options);
};

// eslint-disable-next-line arrow-body-style
export const generateRoomPlanDownloadUrl = (roomItem: RoomDetailsDto | RoomForProjectDto): string => {
  return `${accountStorageUrl}/${containerName}/${generateFilename(roomItem)}${roomItem.sasTokenForRoomPlans}`;
};

const generateFilename = (roomItem: RoomDetailsDto | RoomForProjectDto): string => {
  const cleanRoomName = roomItem.name.replace(new RegExp(' ', 'g'), '').replace(new RegExp('-', 'g'), '');
  return `${roomItem.building.name + cleanRoomName}.jpg`;
};
