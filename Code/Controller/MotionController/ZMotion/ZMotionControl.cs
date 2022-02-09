using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController
{
    /// <summary>
    /// 正运动操作类
    /// </summary>
    public class ZMotionControl : Motor
    {
        public override int Motor_add_arc_pos(int crdID, double[] ppos1, double[] ppos2, double[] ppos3, TSpeed[] pspeed)
        {
            throw new NotImplementedException();
        }

        public override int Motor_add_line_pos(int crdID, double[] ppos, TSpeed pspeed)
        {
            throw new NotImplementedException();
        }

        public override int Motor_axis_disable(ushort cardIndex, ushort axisIndex)
        {
            throw new NotImplementedException();
        }

        public override int Motor_axis_enable(ushort cardIndex, ushort axisIndex)
        {
            throw new NotImplementedException();
        }

        public override int Motor_axis_home(ushort cardIndex, ushort axisIndex, uint homeIo, float speed, float secondSpeed)
        {
            throw new NotImplementedException();
        }

        public override int Motor_axis_home_finished(ushort cardIndex, ushort axisIndex)
        {
            throw new NotImplementedException();
        }

        public override int Motor_axis_is_moving(ushort cardIndex, ushort axisIndex)
        {
            throw new NotImplementedException();
        }

        public override int Motor_axis_max_speed(ushort cardIndex, ushort axisIndex, out TSpeed speed)
        {
            throw new NotImplementedException();
        }

        public override int Motor_axis_move_offset(ushort cardIndex, ushort axisIndex, double offset, TSpeed pspeed, int basePos)
        {
            throw new NotImplementedException();
        }

        public override int Motor_axis_move_pos(ushort cardIndex, ushort axisIndex, double pos, TSpeed pspeed)
        {
            throw new NotImplementedException();
        }

        public override int Motor_axis_reset(ushort cardIndex, ushort axisIndex)
        {
            throw new NotImplementedException();
        }

        public override int Motor_axis_stop(ushort cardIndex, ushort axisIndex, int type)
        {
            throw new NotImplementedException();
        }

        public override int Motor_axis_vmove(ushort cardIndex, ushort axisIndex, int dir, TSpeed pspeed)
        {
            throw new NotImplementedException();
        }

        public override int Motor_clear_crd_data(int crdID)
        {
            throw new NotImplementedException();
        }

        public override int Motor_cmd(string pCmd)
        {
            throw new NotImplementedException();
        }

        public override int Motor_crd_move(int crdID)
        {
            throw new NotImplementedException();
        }

        public override int Motor_create_crd(out ushort pCardIndex, out ushort pAxisIndex, ushort axisCnt)
        {
            throw new NotImplementedException();
        }

        public override int Motor_deinit()
        {
            throw new NotImplementedException();
        }

        public override int Motor_ext_read_in_bit(ushort cardIndex, ushort node, short index)
        {
            throw new NotImplementedException();
        }

        public override int Motor_ext_write_out_bit(ushort cardIndex, short exindex, ushort ioindex, ushort val)
        {
            throw new NotImplementedException();
        }

        public override int Motor_get_axis_status(ushort cardIndex, ushort axisIndex, int type)
        {
            throw new NotImplementedException();
        }

        public override int Motor_get_axis_type(ushort cardIndex, ushort axisIndex)
        {
            throw new NotImplementedException();
        }

        public override int Motor_get_crd_status(int crdID, out short pRun, out long pSegment)
        {
            throw new NotImplementedException();
        }

        public override int Motor_get_current_pos(ushort cardIndex, ushort axisIndex, out double pval, int type = 0)
        {
            throw new NotImplementedException();
        }

        public override int Motor_init_card(AxisParam paxis, ushort cardIndex, ushort axisCnt, int extIOCnt, int cntType)
        {
            throw new NotImplementedException();
        }

        public override int Motor_read_adc(ushort cardIndex, ushort index, out double pvalue, short exindex)
        {
            throw new NotImplementedException();
        }

        public override int Motor_read_in(ushort cardIndex, short di_type, out long pval, short exindex = 0)
        {
            throw new NotImplementedException();
        }

        public override int Motor_read_in_bit(ushort cardIndex, ushort ioindex, short exindex)
        {
            throw new NotImplementedException();
        }

        public override int Motor_read_out(ushort cardIndex, short do_type, out long pval, short exindex)
        {
            throw new NotImplementedException();
        }

        public override int Motor_read_out_bit(ushort cardIndex, ushort ioindex, short exindex, short do_type)
        {
            throw new NotImplementedException();
        }

        public override int Motor_reset()
        {
            throw new NotImplementedException();
        }

        public override bool Motor_set_axis_zero_pos(ushort cardIndex, ushort axisIndex)
        {
            throw new NotImplementedException();
        }

        public override int Motor_write_out_bit(ushort cardIndex, ushort ioindex, ushort val, short exindex, short do_type)
        {
            throw new NotImplementedException();
        }
    }
}
