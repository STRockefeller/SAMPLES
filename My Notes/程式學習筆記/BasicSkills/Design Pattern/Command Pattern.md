# Command Pattern

[Reference:ITHelp](https://ithelp.ithome.com.tw/articles/10204425)

[Reference:NotFalse](https://notfalse.net/4/command-pattern)

## 目的

> 命令模式(Command Pattern)有三個主要角色，
> `Invoker`、`ICommand`和`Receiver`，
> 是將對行爲進行封裝的典型模式，
> 將命令的`命令接收(請求操作者)`跟`執行命令(實際操作者)`之間切分開來。
>
> 幾乎所有的類別都可以套用命令模式，但是只有在需要某些特殊功能，
> 如`記錄操作步驟`、`取消上次命令`的時候，
> 比較適合用命令模式。
>
> > 命令模式有幾個優點：
> >
> > 1. 它能較容易的設計一個命令序列。
> > 2. 在需要的狀況下，可以較容易的將命令記入日誌。
> > 3. 允許接收請求的一方決定是否要否決請求。
> > 4. 可以容易的實現對請求的取消和重做。
> > 5. 由於加進新的具體命令類別不影響其他類別，因此增加新的具體命令類別很容易。
> >
> > 最後、**最大的優點是將請求的物件和執行的物件分開。**
> > -- *大話設計模式 p.355*
>
> ![Command Pattern](https://ithelp.ithome.com.tw/upload/images/20181023/20112528462JLh3GNy.png)

## 概念

> > 試著將控制燈光用命令模式實作。
>
> ```java
> public class Light {
> 	//Receiver可以是任何的類
>     public void turnOn(){
>         System.out.println("打開燈");
>     }
> 
>     public void turnOff(){
>         System.out.println("關燈");
>     }
> 
>     public void brighter(){
>         System.out.println("亮度提高");
>     }
> 
>     public void darker(){
>         System.out.println("亮度降低");
>     }
> 
> }
> ```
>
> > 對燈光控制的Command介面
>
> ```java
> public abstract class Command {
>     
>     Light light;
> 
>     public Command(Light light){
>         this.light = light;
>     }
> 
>     public abstract void execute();
> }
> ```
>
> > 燈光底下的Command
>
> ```java
> public class TurnOn extends  Command {
>     public TurnOn(Light light) {
>         super(light);
>     }
> 
>     @Override
>     public void execute() {
>         light.turnOn();
>     }
> }
> 
> public class TurnOff extends Command {
> 
>     public TurnOff(Light light) {
>         super(light);
>     }
> 
>     @Override
>     public void execute() {
>         light.turnOff();
>     }
> }
> 
> public class Brighter extends Command{
>     public Brighter(Light light) {
>         super(light);
>     }
> 
>     @Override
>     public void execute() {
>         light.brighter();
>     }
> }
> 
> public class Darker extends Command {
>     public Darker(Light light) {
>         super(light);
>     }
> 
>     @Override
>     public void execute() {
>         light.darker();
>     }
> 
> }
> ```
>
> > 燈光的遙控器，可以儲存commands。
>
> ```java
> public class Invoker {
> 
>     private List<Command> commandList = new ArrayList<>();
> 
>     public void addCommand(Command command) {
>         commandList.add(command);
>     }
> 
>     public void execute(){
>         for (Command command :
>                 commandList) {
>             command.execute();
>         }
>     }
> 
> }
> ```
>
> > 測試一下
>
> ```java
> public class Test {
> 
>     @org.junit.jupiter.api.Test
>     public void test(){
> 
>         Light light = new Light();
> 
>         Command turnOn = new TurnOn(light);
>         Command brighter = new Brighter(light);
>         Command darker = new Darker(light);
> 
>         Invoker invoker = new Invoker();
> 
>         invoker.addCommand(turnOn);
>         invoker.addCommand(brighter);
>         invoker.addCommand(brighter);
>         invoker.addCommand(brighter);
>         invoker.addCommand(darker);
> 
>         invoker.execute();
> 
>     }
> 
> }
> ```
>
> > 測試結果
>
> ```
> 打開燈
> 亮度提高
> 亮度提高
> 亮度提高
> 亮度降低
> ```
>
> 實作 - 2
>
> 試著實現魔術方塊的Command模式 ⋯
>
> > 對Tetris Game的操作
>
> ```java
> public class Tetris {
> 
>     public Tetris(){
>     }
> 
>     public void trunLeft(){
>         System.out.println("向左轉");
>     }
> 
>     public void turnRight(){
>         System.out.println("向右轉");
>     }
> 
>     public void straightDown(){
>         System.out.println("直接下降");
>     }
> 
> }
> ```
>
> > 魔術方塊的Command介面
>
> ```java
> public abstract class ICommandTetris {
> 
> //    抽象的命令
>     protected Tetris tetris;
> 
>     public ICommandTetris(Tetris tetris) {
>         this.tetris = tetris;
>     }
> 
>     public abstract void execute();
> 
> }
> ```
>
> > 三種對魔術方塊的操作
>
> ```java
> public class TurnLeft extends ICommandTetris {
> 
>     public TurnLeft(Tetris tetris) {
>         super(tetris);
>     }
> 
>     @Override
>     public void execute() {
>         tetris.trunLeft();
>     }
> }
> 
> public class TurnRight extends ICommandTetris{
> 
>     public TurnRight(Tetris tetris) {
>         super(tetris);
>     }
> 
>     @Override
>     public void execute() {
>         tetris.turnRight();
>     }
> }
> 
> public class StraightDown extends ICommandTetris
> {
>     public StraightDown(Tetris tetris) {
>         super(tetris);
>     }
> 
>     @Override
>     public void execute() {
>         tetris.straightDown();
>     }
> }
> ```
>
> > 遊戲的操縱者
>
> ```java
> public class Invoker {
> 
>     ICommandTetris command;
> 
> 
>     public Invoker(ICommandTetris command){
>         this.command = command;
>     }
> 
>     public void setCommand(ICommandTetris command){
>         this.command = command;
>     }
> 
>     public void invoke(){
>         command.execute();
>     }
> 
> }
> ```
>
> > 測試一下
>
> ```java
> public class Test {
> 
>     @org.junit.jupiter.api.Test
>     public void test(){
> 
>         Tetris tetris = new Tetris();
>         ICommandTetris commandLeft = new TurnLeft(tetris);
>         ICommandTetris commandRight = new TurnRight(tetris);
>         ICommandTetris commandDown= new StraightDown(tetris);
> 
>         Invoker invoker = new Invoker(commandLeft);
> 
>         invoker.invoke();
> 
>         invoker.setCommand(commandRight);
> 
>         invoker.invoke();
> 
>         invoker.setCommand(commandDown);
> 
>         invoker.invoke();
> 
>     }
> 
> }
> ```
>
> > 測試結果
>
> ```java
> 向左轉
> 向右轉
> 直接下降
> ```
>
> 命令模式實現起來沒有很困難，幾乎所有的類別都可以套用命令模式，但是無謂的套用只會增加類別的數量。所以在有適合使用命令模式的需求，到那時再重構就好。



---

> ![](https://s3.notfalse.net/wp-content/uploads/2016/12/24154946/command-pattern-class-diagram1.png)

> - Client (負責建立 具體命令 並組裝 接收者):
>   建立 具體的命令物件 (ConcreteCommand)，
>   並設定其接收者 (Receiver)，
>   此處的 Client 是站在『命令模式』的立場，而非泛指的『客戶』！
>
>  
>
> - Invoker (負責儲存與呼叫命令):
>   儲存 具體的命令物件 (ConcreteCommand) ，
>   並負責呼叫該命令 —— ConcreteCommand.Execute()，
>   若該 Command 有實作 『復原』功能，則在執行之前，先儲存其狀態。
>
>  
>
> - Command (負責制定命令使用介面):
>   如其名，是此模式的關鍵之處 。
>   『至少』會含有一個 Execute() 的抽象操作 (方法) (abstract operation) 。
>
>  
>
> - Receiver (負責執行命令的內容):
>   知道如何根據命令的請求，執行任務內容，
>   因此任何能實現命令請求的類別，都有可能當作 Receiver。
>
>  
>
> - ConcreteCommand (負責呼叫 Receiver 的對應操作):
>   具體 的命令類別，
>   通常持有 Receiver 物件。

## 實作

先整理重點，Command Pattern的構成

至少有分為Invoker ICommand Command Receiver

在NotFalse裡面還多提了Client，不過看來並不是必要項目

比較混淆的地方是執行的部分，Command裡面會包含Reciever物件，Command裡面的執行方法會令該Receiver物件執行相應的動作

---

### 基本架構

先實作基本架構試試看

Class Diagram

![](https://i.imgur.com/80r0gbw.png)

內容物

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandPattern
{
    public interface ICommand
    {
        void Execute();
    }

    public class Invoker
    {
        private List<Command> commands;
        public Invoker()
        {
            commands = new List<Command>();
        }
        public void addCommand(Command command) => commands.Add(command);
        public void removeCommand(Command command) => commands.Remove(command);
        public void executeCommands()
        {
            foreach (Command command in commands)
                command.Execute();
        }
    }

    public class Command : ICommand
    {
        Receiver receiver;
        public Command(Receiver receiver) => this.receiver = receiver;
        public void Execute() => receiver.action();
    }

    public class Receiver
    {
        public void action() { }
    }
}
```

---

### 應用情境

想個應用情境:假如我要下指令控制一台工具機進行加工

先分配一下每個部件扮演的角色

* Command:我所下的各種指令，如水機啟動、主軸啟動、軸向移動等等
* Receiver:實際執行動作的物件，在這個情境下應為工具機本身
* Invoker:儲存指令以執行指令內容，應為控制器

為了增加挑戰性試著實作undo功能，雖然實際工具機不可能有這個功能就是了

Class Diagram

![](https://i.imgur.com/DVPgMZc.png)

內容

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandPattern
{
    public interface ICommand
    {
        void Execute(ref Machine machine);
    }

    public class Controller
    {
        private List<ICommand> commands;
        private Machine machine;
        private Machine previousStatus;
        public Controller(Machine machine)
        {
            commands = new List<ICommand>();
            this.machine = machine;
            previousStatus = machine.backup();
        }
        public void addCommand(ICommand command) => commands.Add(command);
        public void removeCommand(ICommand command) => commands.Remove(command);
        public Machine executeCommands()
        {
            foreach (ICommand command in commands)
                command.Execute(ref machine);
            return machine;
        }
        public Machine undo() => previousStatus;
    }

    public class HydraulicCommand : ICommand
    {
        public void Execute(ref Machine machine) => machine.exeHydraulic();
    }
    public class CoolantCommand : ICommand
    {
        public void Execute(ref Machine machine) => machine.exeCoolant();
    }

    public class MagneticCommand : ICommand
    {
        public void Execute(ref Machine machine) => machine.exeMagnetic();
    }

    public class SpindleCommand : ICommand
    {
        public void Execute(ref Machine machine) => machine.exeSpindle();
    }
    public class Machine
    {
        bool isHydraulicOn = false;
        bool isCoolantOn = false;
        bool isSpindleOn = false;
        bool isMagneticOn = false;
        public void exeHydraulic()
        {
            if (isHydraulicOn == false)
            {
                isHydraulicOn = true;
                Console.WriteLine("油壓啟動");
            }
            else
            {
                isHydraulicOn = false;
                Console.WriteLine("油壓關閉");
            }
        }
        public void exeCoolant()
        {
            if (isCoolantOn == false)
            {
                isCoolantOn = true;
                Console.WriteLine("水機啟動");
            }
            else
            {
                isCoolantOn = false;
                Console.WriteLine("水機關閉");
            }
        }
        public void exeSpindle()
        {
            if (isSpindleOn == false)
            {
                isSpindleOn = true;
                Console.WriteLine("主軸運轉");
            }
            else
            {
                isSpindleOn = false;
                Console.WriteLine("主軸停止");
            }
        }
        public void exeMagnetic()
        {
            if (isMagneticOn == false)
            {
                isMagneticOn = true;
                Console.WriteLine("吸磁");
            }
            else
            {
                isMagneticOn = false;
                Console.WriteLine("脫磁");
            }
        }
        public void describe()
        {
            string res = "機台狀態\r\n油壓:";
            res += isHydraulicOn ? "啟動中" : "未啟動";
            res += "\r\n水機:";
            res += isCoolantOn ? "啟動中" : "未啟動";
            res += "\r\n主軸:";
            res += isSpindleOn ? "啟動中" : "未啟動";
            res += "\r\n磁鐵:";
            res += isMagneticOn ? "啟動中" : "未啟動";
            Console.WriteLine(res);
        }
        public Machine backup() => new Machine()
        {
            isCoolantOn = this.isCoolantOn,
            isHydraulicOn = this.isHydraulicOn,
            isMagneticOn = this.isMagneticOn,
            isSpindleOn = this.isSpindleOn
        };
    }
}
```

因為想讓機台物件唯一所以稍微更動了下結構，在Command裡面不再實例化Receiver，改成在Execute方法裡面傳入ref Machine

測試程式碼

```C#
        private void commandTest()
        {
            Console.WriteLine("新增機台物件");
            Machine machine = new Machine();
            machine.describe();
            Controller controller = new Controller(machine);
            controller.addCommand(new MagneticCommand());
            controller.addCommand(new HydraulicCommand());
            controller.addCommand(new SpindleCommand());
            controller.addCommand(new HydraulicCommand());
            controller.addCommand(new CoolantCommand());
            Console.WriteLine("執行Command");
            machine = controller.executeCommands();
            machine.describe();
            Console.WriteLine("undo");
            machine = controller.undo();
            machine.describe();
        }
```

結果

> 新增機台物件
> 機台狀態
> 油壓:未啟動
> 水機:未啟動
> 主軸:未啟動
> 磁鐵:未啟動
> 執行Command
> 吸磁
> 油壓啟動
> 主軸運轉
> 油壓關閉
> 水機啟動
> 機台狀態
> 油壓:未啟動
> 水機:啟動中
> 主軸:啟動中
> 磁鐵:啟動中
> undo
> 機台狀態
> 油壓:未啟動
> 水機:未啟動
> 主軸:未啟動
> 磁鐵:未啟動

#### 分析與檢討

Machine類別其實理想是以單例模式設計，但由於undo功能的設計，多實例化一個物件作為上一個ˋ狀態的儲存



考慮一下是否有其他實現undo的方法

在這個例子中，簡單的作法的話也可以讓undo再次執行Execute，如下

```C#
    public class Controller
    {
        private List<ICommand> commands;
        private Machine machine;
        public Controller(Machine machine)
        {
            commands = new List<ICommand>();
            this.machine = machine;
            previousStatus = machine.backup();
        }
        public void addCommand(ICommand command) => commands.Add(command);
        public void removeCommand(ICommand command) => commands.Remove(command);
        public Machine executeCommands()
        {
            foreach (ICommand command in commands)
                command.Execute(ref machine);
            return machine;
        }
        public Machine undo() => executeCommand();
    }
```

當然這麼做沒有Execute之前先做undo就會很奇怪



閱讀NotFalse文章，得知兩種做法

> 1. 新增一個 具體命令類別，讓 execute() 呼叫復原邏輯。
> 2. 在 抽象命令類別 (or 介面) 中，新增反向操作 unExecute()，來呼叫復原邏輯。

可以看到，不論是哪一種做法都是在Command上作文章，而非Invoker。

```C#
public class Jas
{
    public string hello(){}
}
```





另一個問題就是耦合嚴重，Command依賴於Receiver在架構中可能是不可避免的，但是在我這個程式中Invoker也依賴於Receiver

會設計成這樣的原因是希望Machine物件(Receiver)的狀態要隨著Command被Invoke而改變，