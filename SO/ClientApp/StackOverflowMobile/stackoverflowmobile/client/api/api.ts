export * from './posts.service';
import { PostsService } from './posts.service';
export * from './users.service';
import { UsersService } from './users.service';
export const APIS = [PostsService, UsersService];
