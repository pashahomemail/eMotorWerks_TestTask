var findedId = false;
var getUrl = window.location;
//var baseUrl = getUrl.protocol + "//" + getUrl.host + getUrl.pathname;
var href = window.location.href;
var indexOf = href.indexOf("?id=");
if(indexOf !== '-1'){
    findedId = true;
}
var id = href.substring(indexOf+4, href.length);


var groupDetails = new Vue({
    el: '#groupDetails',
    data: {
        found: findedId,
        group: {
            id: 0,
            name: ''
        },
        parents: null,
        childrens: null,
        freeParents: null,
        freeChildrens: null,
        free:null,
        error: null,
    },
    created(){
        this.fetchData()
    },
    methods: {
        //some methods
        fetchData () {
            var that = this;
            this.error = null;
            axiosInstance.get("groups/" + id).then(function(response){
                that.found = true;
                that.group = response.data; 
                that.parents = that.group.parents;
                that.childrens = that.group.childrens;   
                that.freeParents = that.group.freeParents;
                that.freeChildrens = that.group.freeChildrens; 
                that.free = that.group.free;
            },function(error){
                that.error = error;
                that.found = false;
            });
        },
        addGroupToParent(groupId, parentId){
            var that = this;
            axiosInstance.post("groups/parent", {childrenId:groupId, parentId:parentId}).then(function(response){
                that.fetchData();                                
            },function(error){
                that.fetchData();
            });
        },
        removeGroupFromParentGroup(groupId, parentId){
            var that = this;
            axiosInstance.delete("groups/" + groupId + "/parent/" + parentId).then(function(response){
                that.fetchData();                                
            },function(error){
                that.fetchData();
            });
        },
        addGroupToChildren(groupId, parentId){
            var that = this;
            axiosInstance.post("groups/children", {childrenId:parentId, parentId:groupId}).then(function(response){
                that.fetchData();                                
            },function(error){
                that.fetchData();
            });
        },
        removeGroupFromChildrenGroup(groupId, childrenId){
            var that = this;
            axiosInstance.delete("groups/" + groupId + "/children/" + childrenId).then(function(response){
                that.fetchData();                                
            },function(error){
                that.fetchData();
            });
        }
    }
});