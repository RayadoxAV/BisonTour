


function log(message) {

  const colors = [
    {
      value: '',
      name: 'INFO'
    },
    {
      value: '\x1b[32m',
      name: 'SUCCESS'
    },
    {
      value: '\x1b[33m',
      name: 'WARNING'
    },
    {
      value: '\x1b[31m',
      name: 'ERROR'
    }
  ];

  const reset = '\x1b[0m';

  console.log(`${colors[1].value}%s${reset}`, `${colors[1].name}: ${message}`);
}

console.clear();
console.log('Running tests for: PlayerController.cs');
log('Jumping............................✔️');
log('--Jump Start.......................✔️');
log('--Jump.............................✔️');
log('--Jump End.........................✔️');
log('Move camera........................✔️');
log('--Far angle........................✔️');
log('--Close angle......................✔️');
log('Movevement.........................✔️');
log('--Walking..........................✔️');
log('--Running..........................✔️');
log('Crouch.............................✔️');
log('Collision..........................✔️');
log('--Frontal collision................✔️');
log('--Vertical collision...............✔️')