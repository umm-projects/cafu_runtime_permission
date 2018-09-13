const fs = require('fs');
const path = require('path');
const umm = require('@umm/scripts');

if (umm.libraries.info.development_install) {
  return;
}

// Assets/Modules/umm@cafu_runtime_permission/Plugins/ 以下全てを Assets/Plugins/ 以下に移動
umm.libraries.synchronize(
  path.join(umm.libraries.info.base_path, 'Assets', 'Modules', umm.libraries.info.module_name, 'Plugins'),
  path.join(umm.libraries.info.base_path, 'Assets', 'Plugins'),
  [
    '**/*',
  ],
  {
    overwrite: false,
    remove_source: true,
    remove_empty_source_directory: true,
  }
);
