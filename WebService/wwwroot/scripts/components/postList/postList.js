define(['knockout', 'dataservice'], function (ko, db) {
    return function (params) {
        var self = this;
        this.page = params.page || ko.observable(0);
        this.next = ko.observable();
        this.prev = ko.observable();
        this.hasNext = ko.computed(function() {
            return self.next() || false;
        }, this);
        this.hasPrev = ko.computed(function () {
            return self.prev() || false;
        }, this);
        this.posts = ko.observableArray([]);
      /*  this.getPosts = function(link) {
            console.log(`test: ${link}`);
            $.ajax({
                url: link,
                success: function(result) {
                    console.log(result);
                    self.posts(result.items);
                    self.prev(result.prev);
                    self.next(result.next);
                }
            });
        }*/
        //this.getPosts(`/api/posts?page=${this.page()}`);

        this.getPosts = function (link) {
            console.log(`test: ${link}`);
            let result = db.getPosts("sql", self.page);
            self.posts(result.results);
            self.prev(result.previousPage);
            self.next(result.nextPage);
        }
        this.getPosts('');
        this.gotoNext = function() {
            if (self.hasNext()) {
                self.getPosts(self.next());
                self.page(self.page()+1);
            }
        }
        this.gotoPrev = function () {
            if (self.hasPrev()) {
                self.getPosts(self.prev());
                self.page(self.page() - 1);
            }
        }
        /*this.getPost = function (postListItem) {
            console.log(postListItem.url);
            $.ajax({
                url: postListItem.url,
                success: function (result) {
                    history.pushState({link: result.url}, "", "");
                    params.post(result);
                }
            });
        }*/
        this.getPost = function (postListItem) {
            console.log(postListItem.url);
            console.log(db.getPost(19).title);
        }
        return {
            hasNext: self.hasNext,
            hasPrev: self.hasPrev,
            gotoNext: self.gotoNext,
            gotoPrev: self.gotoPrev,
            posts: self.posts,
            page: self.page,
            getPost: self.getPost
        };

    }
});