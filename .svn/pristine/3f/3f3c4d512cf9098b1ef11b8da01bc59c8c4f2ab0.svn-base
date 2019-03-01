<template>
    <div>
        <el-tree
                :data="treedata"
                show-checkbox
                node-key="id"
                :props="defaultProps">
        </el-tree>
    </div>
</template>

<script>
    import draggable from 'vuedraggable'

    export default {
        name: "tuozhuai",
        components: {
            draggable
        },
        data() {
            return {
                treedata:[],
                defaultProps: {
                    children: 'children',
                    label: 'label'
                },
                alldata:[]
            }
        },
        created(){
          this.init()
        },
        methods: {
            init(){
                this.axios.get(`/api/UserRightMange/GetMenus`).then(res=>{
                    console.log(res)
                    if (res.data.code="000000"){
                        res.data.Data.map(item=>{
                            this.alldata.push(item)
                        })
                        res.data.Datas.map(list=>{
                            this.alldata.push(list)
                        })
                        console.log(this.alldata)
                        this.toTreeData(this.alldata, 'id', 'p_id', 'title')
                        console.log(this.treedata)
                    }
                })

            },
            // 循环出父节点
            toTreeData(data, id, pid, name) {
                // 建立个树形结构,需要定义个最顶层的父节点，pId是1
                let leve = 0
                let parent = [];
                this.treedata = parent
                for (let i = 0; i < data.length; i++) {
                    if (data[i][pid] !== 0) {
                    } else {
                        let obj = {
                            label: data[i][name],
                            id: data[i][id],
                            parent_id: data[i][pid],
                            leve: leve,
                            children: []
                        };
                        parent.push(obj);//数组加数组值
                    }
                    // console.log(obj);
                    //  console.log(parent,"bnm");
                }
                children(parent, leve);

                // 调用子节点方法,参数为父节点的数组
                function children(parent, num) {
                    num++;
                    if (data.length !== 0 && num != 4) {
                        for (let i = 0; i < parent.length; i++) {
                            for (let j = 0; j < data.length; j++) {
                                if (parent[i].id == data[j][pid]) {
                                    let obj = {
                                        label: data[j][name],
                                        id: data[j][id],
                                        parent_id: data[j][pid],
                                        leve: num,
                                        children: []
                                    };
                                    parent[i].children.push(obj);
                                }
                            }
                            children(parent[i].children, num);
                        }
                    }
                    return num
                }
                console.log(parent, "bjil")
                return parent;
            },
        }
    }
</script>

<style scoped>

</style>