namespace bashforms_tests.todo_scenario
{
    /*
     * Manage a list of to-do items.
     *
     * Functionality:
     * - List all to-do items.
     * - Filter to-do items according to a search pattern.
     * - Delete single item.
     * - Add new item.
     * - Edit item.
     *
     * To-do item data:
     * - Subject
     * - Description
     * - Due date/time
     * - Priority
     * - Tags
     *
     * A list of tags is displayed for easy filtering. It's updated whenever an item is stored/deleted.
     *
     * A list of filtered items is displayed. Initially all items are shown.
     *
     * To edit an item a new dialog is opened.
     *
     * To-do item persistence:
     * The items are stored as JSON files in a local repository folder.
     * They are all kept in memory; changes are written thru to the file system.
     * 
     */
    public class ToDoApp
    {
        public void Run()
        {
            
        }
    }
}