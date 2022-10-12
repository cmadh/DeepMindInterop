namespace DeepMindInterop.Extensions
{
    public enum DeepMindEventType
    {
        DeepMindVersion,
        AbiDumpStart,
        AbiDumpAbi,
        AbiDumpEnd,
        StartBlock,
        SwitchForks,
        Error,
        OnBlock,
        AcceptedBlock,
        AppliedTransaction,
        AddRamCorrection,
        InputAction,
        RequireRecipient,
        SendInline,
        SendContextFreeInline,
        CancelDeferred,
        FailDeferred,
        CreateTable,
        RemoveTable,
        DbStorei64,
        DbUpdatei64,
        DbRemovei64,
        RamEvent,
        CreatePermission,
        ModifyPermission,
        RemovePermission
    }

    public class DeepMindEventData
    {
        public DeepMindEventType EventType;

        public object Data;

        public DeepMindEventData(DeepMindEventType eventType, object data)
        {
            EventType = eventType;
            Data = data;
        }
    }

    public class DeepMindInteropLogger : SwigLoggerBase
    {
        public static uint CurrentBlock { get; private set; }
        List<DeepMindEventData> deepMindEvents = new List<DeepMindEventData>();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void on_deep_mind_version(string name, uint major, uint minor)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.DeepMindVersion, new DeepMindVersion(name, major, minor)));
        }

        public override void on_abidump_start(uint block_num, ulong global_sequence_num)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.AbiDumpStart, new AbiDumpStart(block_num, global_sequence_num)));
        }

        public override void on_abidump_abi(ulong name, IntPtr abi_data)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.AbiDumpAbi, new AbiDumpAbi(name, MemoryHelper.Copy(abi_data))));
        }

        public override void on_abidump_end()
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.AbiDumpEnd, new AbiDumpEnd()));
        }

        public override void on_start_block(uint start_block)
        {
            deepMindEvents.Clear();
            CurrentBlock = start_block;
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.StartBlock, new StartBlock(start_block)));
        }

        public override void on_accepted_block(uint num, IntPtr blk)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.AcceptedBlock, new AcceptedBlock(num, MemoryHelper.Copy(blk))));
        }

        public override void on_switch_forks(IntPtr from_id, IntPtr to_id)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.SwitchForks, new SwitchForks(MemoryHelper.Copy(from_id), MemoryHelper.Copy(to_id))));
        }

        public override void on_error(IntPtr id, IntPtr trx)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.Error, new Error(MemoryHelper.Copy(id), MemoryHelper.Copy(trx))));
        }

        public override void on_onblock(IntPtr id, IntPtr trx)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.OnBlock, new OnBlock(MemoryHelper.Copy(id), MemoryHelper.Copy(trx))));
        }

        public override void on_applied_transaction(uint block_num, IntPtr traces)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.AppliedTransaction, new AppliedTransaction(block_num, MemoryHelper.Copy(traces))));
        }

        public override void on_add_ram_correction(uint action_id, long correction_id, string event_id, ulong payer, ulong delta)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.AddRamCorrection, new AddRamCorrection(action_id, correction_id, event_id, payer, delta)));
        }

        public override void on_input_action(uint action_id)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.InputAction, new InputAction(action_id)));
        }

        public override void on_require_recipient(uint action_id)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.RequireRecipient, new RequireRecipient(action_id)));
        }

        public override void on_send_inline(uint action_id)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.SendInline, new SendInline(action_id)));
        }

        public override void on_send_context_free_inline(uint action_id)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.SendContextFreeInline, new SendContextFreeInline(action_id)));
        }

        public override void on_cancel_deferred(byte qual, uint action_id, ulong sender, IntPtr sender_id, ulong payer, uint published,
            uint delay, uint expiration, IntPtr trx_id, IntPtr trx)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.CancelDeferred, new CancelDeferred(qual, action_id, sender, MemoryHelper.Copy(sender_id), payer, published, delay, expiration, MemoryHelper.Copy(trx_id), MemoryHelper.Copy(trx))));
        }

        public override void on_send_deferred(byte qual, uint action_id, ulong sender, IntPtr sender_id, ulong payer, uint published,
            uint delay, uint expiration, IntPtr trx_id, IntPtr trx)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.CancelDeferred, new CancelDeferred(qual, action_id, sender, MemoryHelper.Copy(sender_id), payer, published, delay, expiration, MemoryHelper.Copy(trx_id), MemoryHelper.Copy(trx))));
        }

        public override void on_create_deferred(byte qual, uint action_id, ulong sender, IntPtr sender_id, ulong payer, uint published,
            uint delay, uint expiration, IntPtr trx_id, IntPtr trx)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.CancelDeferred, new CancelDeferred(qual, action_id, sender, MemoryHelper.Copy(sender_id), payer, published, delay, expiration, MemoryHelper.Copy(trx_id), MemoryHelper.Copy(trx))));
        }

        public override void on_fail_deferred(uint action_id)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.FailDeferred, new FailDeferred(action_id)));
        }

        public override void on_create_table(uint action_id, ulong code, ulong scope, ulong table, ulong payer)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.CreateTable, new CreateTable(action_id, code, scope, table, payer)));
        }

        public override void on_remove_table(uint action_id, ulong code, ulong scope, ulong table, ulong payer)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.RemoveTable, new RemoveTable(action_id, code, scope, table, payer)));
        }

        public override void on_db_store_i64(uint action_id, ulong payer, ulong table_code, ulong scope, ulong table_name, ulong primkey,
            IntPtr ndata)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.DbStorei64, new DbStorei64(action_id, payer, table_code, table_name, primkey, MemoryHelper.Copy(ndata))));
        }

        public override void on_db_update_i64(uint action_id, ulong payer, ulong table_code, ulong scope, ulong table_name, ulong primkey,
            IntPtr odata, IntPtr ndata)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.DbUpdatei64, new DbUpdatei64(action_id, payer, table_code, table_name, primkey, MemoryHelper.Copy(odata), MemoryHelper.Copy(ndata))));
        }

        public override void on_db_remove_i64(uint action_id, ulong payer, ulong table_code, ulong scope, ulong table_name, ulong primkey,
            IntPtr odata)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.DbRemovei64, new DbRemovei64(action_id, payer, table_code, table_name, primkey, MemoryHelper.Copy(odata))));
        }

        public override void on_ram_event(uint action_id, string event_id, string family, string operation, string legacy_tag, ulong payer,
            ulong new_usage, long delta)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.RamEvent, new RamEvent(action_id, event_id, family, operation, legacy_tag, payer, new_usage, delta)));
        }

        public override void on_create_permission(uint action_id, long permission_id, IntPtr data)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.CreatePermission, new CreatePermission(action_id, permission_id, MemoryHelper.Copy(data))));
        }

        public override void on_modify_permission(uint action_id, long permission_id, IntPtr opdata, IntPtr npdata)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.ModifyPermission, new ModifyPermission(action_id, permission_id, MemoryHelper.Copy(opdata), MemoryHelper.Copy(npdata))));
        }

        public override void on_remove_permission(uint action_id, long permission_id, IntPtr data)
        {
            deepMindEvents.Add(new DeepMindEventData(DeepMindEventType.RemovePermission, new RemovePermission(action_id, permission_id, MemoryHelper.Copy(data))));
        }
    }

    public record RemovePermission(uint ActionId, long PermissionId, byte[] OldPermissionBytes);

    public record ModifyPermission(uint ActionId, long PermissionId, byte[] OldPermissionBytes, byte[] NewPermissionBytes);

    public record CreatePermission(uint ActionId, long PermissionId, byte[] NewPermissionBytes);

    public record RamEvent(uint ActionId, string EventId, string Family, string Operation, string LegacyTag, ulong Payer, ulong NewUsage, long Delta);

    public record DbRemovei64(uint ActionId, ulong Payer, ulong TableCode, ulong TableName, ulong Primkey, byte[] OldDataBytes);

    public record DbUpdatei64(uint ActionId, ulong Payer, ulong TableCode, ulong TableName, ulong Primkey, byte[] OldDataBytes, byte[] NewDataBytes);

    public record DbStorei64(uint ActionId, ulong Payer, ulong TableCode, ulong TableName, ulong Primkey, byte[] NewDataBytes);

    public record RemoveTable(uint ActionId, ulong Code, ulong Scope, ulong Table, ulong Payer);

    public record CreateTable(uint ActionId, ulong Code, ulong Scope, ulong Table, ulong Payer);

    public record FailDeferred(uint ActionId);

    public record CancelDeferred(byte Qual, uint ActionId, ulong Sender, byte[] SenderIdBytes, ulong Payer, uint Published, uint Delay, uint Expiration, byte[] TrxIdBytes, byte[] TrxBytes);

    public record SendContextFreeInline(uint ActionId);

    public record SendInline(uint ActionId);

    public record RequireRecipient(uint ActionId);

    public record InputAction(uint ActionId);

    public record AddRamCorrection(uint ActionId, long CorrectionId, string EventId, ulong Payer, ulong Delta);

    public record AppliedTransaction(uint BlockNum, byte[] TracesBytes);

    public record SwitchForks(byte[] FromBytes, byte[] ToBytes);

    public record AcceptedBlock(uint Num, byte[] BlockBytes);

    public record Error(byte[] IdBytes, byte[] TrxBytes);

    public record OnBlock(byte[] IdBytes, byte[] BlockBytes);

    public record StartBlock(uint BlockNum);

    public record AbiDumpEnd;

    public record AbiDumpAbi(ulong Name, byte[] Bytes);

    public record AbiDumpStart(uint BlockNum, ulong GlobalSequenceNum);

    public record DeepMindVersion(string Name, uint Major, uint Minor);
}
