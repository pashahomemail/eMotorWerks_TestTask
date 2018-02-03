Vue.component('modal', {
    template: '#modal-template',
    props: ['show'],
    methods: {
        close: function () {
            this.$emit('close');
        },
        newGroup: function () {
            // Some save logic goes here...
            var that = this;
            axiosInstance.post("groups", { name: name }).then(function(response){
                that.$root.fetchData();
            },function(error){
                alert('cant add new group!')
            });
            that.close();
        }
    }
});

var listOfGroupsVue = new Vue({
    el: '#listOfGroups',
    data: {
        showModal: false,
        groups:null,
        errors: null,
        loading: null
    },
    created () {        
        this.fetchData()
    },
    methods: {
        fetchData () {
            var that = this;
            that.error = this.groups === null;
            that.loading = true;
            // replace `getPost` with your data fetching util / API wrapper
            axiosInstance.get("groups").then(function(response){
                that.groups = response.data; 
                that.loading = false;
            },function(error){
                that.users = '';
                that.error = error;
                that.loading = false;
            });
            
        },
        removeData(id){
            var that = this;
            if(!id){
                console.warn('no id');
                return;
            }
            this.error = this.groups === null;
            this.loading = true;
            // replace `getPost` with your data fetching util / API wrapper
            axiosInstance.delete("groups/" + id).then(function(response){
                that.fetchData();                               
            },function(error){
                this.error = error;
                this.loading = false;
            });
            
        }
    }
});