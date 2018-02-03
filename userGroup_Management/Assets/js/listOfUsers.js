//register modal element in vue
Vue.component('modal', {
    template: '#modal-template',
    props: ['show'],
    methods: {
        close: function () {
            this.$emit('close');
        },
        newUser: function () {
            // Some save logic goes here...
            var that = this;
            axiosInstance.post("users", { name: name }).then(function(response){
                that.$root.fetchData();
            },function(error){
                alert('cant add new user!')
            });
            that.close();
        }
    }
});

var listOfUsersVue = new Vue({
    el: '#listOfUsers',
    data: {
        showModal: false,
        users:null,
        errors: null,
        loading: null
    },
    created () {        
        this.fetchData()
    },
    methods: {
        fetchData () {
            var that = this;
            this.error = this.users === null;
            this.loading = true;
            // replace `getPost` with your data fetching util / API wrapper
            axiosInstance.get("users").then(function(response){
                that.loading = false;
                that.users = response.data;                                
            },function(error){
                this.error = error;
                this.loading = false;
            });
        },
        removeData(id){
            var that = this;
            if(!id){
                console.warn('no id');
                return;
            }
            this.error = this.users === null;
            this.loading = true;
            // replace `getPost` with your data fetching util / API wrapper
            axiosInstance.delete("users/" + id).then(function(response){
                that.fetchData();                               
            },function(error){
                this.error = error;
                this.loading = false;
            });
            
        }
    }
});