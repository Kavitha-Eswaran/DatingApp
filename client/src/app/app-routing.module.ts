import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

//Represents a route configuration for the Router service. 
//An array of Route objects, used in Router.config and for nested route configurations in Route.children.
const routes: Routes = [
  //when we say empty string in path, when somebody browses to localhost:4200 directly, 
  //then this is the component that's going to be loaded.
  { path: '', component: HomeComponent },
  //specify the canActivate property and then we can pass this an array of our guards and we only have one i.e AuthGuard. This to prevent the navigation when the user has not logged in (canActivate returns false).
  //Please do not think of this as security. There is no such thing as security on a client side application, but we do what we can to prevent users from doing certain things so long as they're using our application normally.
  // {path: 'members', component: MemberListComponent, canActivate: [AuthGuard]},
  // {path: 'members/:id', component: MemberDetailComponent},
  // {path: 'lists', component: ListsComponent},
  // {path: 'messages', component: MessagesComponent},

  //There is nothing to stop us adding the auth guard onto all of these paths. But just for the sake of demonstration, rather than mentioning canActivate: [AuthGuard] to all these component navigations individually, we can merge them under a single canActivate: [AuthGuard] as below. we want to run the guard and resolver on every execution, so runGuardsAndResolvers: 'always'.
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'members', component: MemberListComponent, canActivate: [AuthGuard]},
      { path: 'members/:id', component: MemberDetailComponent },
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent },
    ]
  },
  //Keeping this error component outside as we don't want this to be hidden under the children. We want to test the authentication response as well.
  { path: 'errors', component: TestErrorsComponent },
  { path: 'not-found', component: NotFoundComponent },
  {path: 'server-error', component: ServerErrorComponent},
  //The path-matching strategy, one of 'prefix' or 'full'. Default is 'prefix'.

  //By default, the router checks URL elements from the left to see if the URL matches 
  //a given path and stops when there is a config match. Importantly there must still be a 
  //config match for each segment of the URL. 
  //For example, '/team/11/user' matches the prefix 'team/:id' if one of the route's children matches the segment 'user'.
  //That is, the URL '/team/11/user matches the config{path: 'team/:id', children: [{path: ':user', component: User}]}
  //but does not match when there are no children as in{path: 'team/:id', component: Team}`.

  //The path-match strategy 'full' matches against the entire URL. It is important to do this when redirecting empty-path routes.
  //Otherwise, because an empty path is a prefix of any URL, the router would apply the redirect even when navigating to the redirect destination, creating an endless loop.
  { path: '**', component: NotFoundComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
