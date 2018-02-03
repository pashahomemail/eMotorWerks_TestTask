var findedId = false;
var getUrl = window.location;
//var baseUrl = getUrl.protocol + "//" + getUrl.host + getUrl.pathname;
var href = window.location.href;
var indexOf = href.indexOf("?id=");
if(indexOf !== '-1'){
    findedId = true;
}
var id = href.substring(indexOf+4, href.length);




var userDetails = new Vue({
    el: '#userDetails',
    data: {
        found:findedId,
        user: {
            id: 0,
            name: '',
            userGroups: null
        },
        groups:null,
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
            axiosInstance.get("users/" + id).then(function(response){
                that.found = true;
                that.user = response.data;                                
            },function(error){
                that.error = error;
                that.found = false;
            });
            axiosInstance.get("users/" + id +"/free").then(function(response){
                that.groups = response.data;                                
            },function(error){
                that.error = error;
            });
        },
        addUserToGroup(userId, groupId){
            var that = this;
            axiosInstance.post("users/addGroup", {userId:userId, groupId:groupId}).then(function(response){
                that.fetchData();                                
            },function(error){
                that.fetchData();
            });
        },
        removeUserFromGroup(userId, groupId){
            var that = this;
            axiosInstance.delete("users/" + id + "/removeGroup/" + groupId).then(function(response){
                that.fetchData();                                
            },function(error){
                that.fetchData();
            });
        }
    }
});